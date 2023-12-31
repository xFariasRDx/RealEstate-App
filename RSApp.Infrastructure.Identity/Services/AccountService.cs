﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RSApp.Core.Services.Contracts;
using RSApp.Core.Services.Dtos;
using RSApp.Core.Services.Dtos.Account;
using RSApp.Core.Services.Enums;
using RSApp.Core.Services.Helpers;
using RSApp.Core.Services.ViewModels.SaveVm;
using RSApp.Infrastructure.Identity.Entities;
using RSApp.Infrastructure.Identity.Extensions;
using RSApp.Infrastructure.Identity.interfaces;
using RSApp.Infrastructure.Identity.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RSApp.Infrastructure.Identity.Services;
public class AccountService : IAccountService {
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly IEmailService _emailService;
  private readonly IRequestService _requestService;
  private readonly IJwtService _jwtService;

  public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService, IRequestService requestService, IJwtService jwtService) {
    _userManager = userManager;
    _signInManager = signInManager;
    _emailService = emailService;
    _requestService = requestService;
    _jwtService = jwtService;
  }
  public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, bool fromApi = false) {

    AuthenticationResponse response = new();

    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null) {
      response.HasError = true;
      response.Error = $"No Accounts registered with {request.Email}";
      return response;
    }

    var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
    if (!result.Succeeded) {
      response.HasError = true;
      response.Error = $"Invalid credentials for {request.Email}";
      return response;
    }
    if (!user.EmailConfirmed) {
      response.HasError = true;
      response.Error = $"The account {request.Email} was deactivated temporarily";
      return response;
    }

    if (fromApi) {
      JwtSecurityToken token = await _jwtService.GenerateJwToken(user);
      response.JWToken = new JwtSecurityTokenHandler().WriteToken(token);
      response.RefreshToken = _jwtService.GenerateRefreshToken().Token;
    }

    response.Id = user.Id;
    response.Email = user.Email;
    response.UserName = user.UserName;


    var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

    response.Roles = rolesList.ToList();
    response.IsVerified = user.EmailConfirmed;

    return response;
  }

  public async Task SignOutAsync() => await _signInManager.SignOutAsync();

  public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin) {
    RegisterResponse response = new() {
      HasError = false
    };

    var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
    if (userWithSameUserName != null) {
      response.HasError = true;
      response.Error = $"username '{request.UserName}' is already taken.";
      return response;
    }

    var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
    if (userWithSameEmail != null) {
      response.HasError = true;
      response.Error = $"Email '{request.Email}' is already registered.";
      return response;
    }

    var user = new ApplicationUser {
      Email = request.Email,
      FirstName = request.FirstName,
      LastName = request.LastName,
      UserName = request.UserName,
      DNI = request.DNI,
      PhoneNumber = request.PhoneNumber,
      Image = request.Image,
    };

    if (!string.IsNullOrWhiteSpace(request.Password)) {
      var passwordValidator = new PasswordValidator<ApplicationUser>();
      var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, request.Password);

      if (!passwordValidationResult.Succeeded) {
        response.HasError = true;
        response.Error = passwordValidationResult.Errors.First().Description;
        return response;
      }

      user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
    }

    var result = await _userManager.CreateAsync(user, request.Password);
    if (result.Succeeded) {
      await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(Roles), request.Role));
      response.UserId = user.Id;
    } else {
      response.HasError = true;
      response.Error = $"An error occurred trying to register the user.";
      return response;
    }
    return response;
  }

  public async Task<string> ConfirmAccountAsync(string userId, string token) {
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null) {
      return $"No accounts registered with this user";
    }

    token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
    var result = await _userManager.ConfirmEmailAsync(user, token);
    if (result.Succeeded) {
      return $"Account confirmed for {user.Email}. You can now use the app";
    } else {
      return $"An error occurred while confirming {user.Email}.";
    }
  }

  public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin) {
    ForgotPasswordResponse response = new() {
      HasError = false
    };

    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user == null) {
      response.HasError = true;
      response.Error = $"No Accounts registered with {request.Email}";
      return response;
    }

    var verificationUri = await _requestService.SendForgotPassword(user, origin);

    await _emailService.SendEmail(new EmailRequest() {
      To = user.Email,
      Body = EmailRequests.ResetPassword(user.UserName, verificationUri),
      Subject = "reset password"
    });


    return response;
  }

  public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request) {
    ResetPasswordResponse response = new() {
      HasError = false
    };

    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user == null) {
      response.HasError = true;
      response.Error = $"No Accounts registered with {request.Email}";
      return response;
    }

    request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
    var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

    if (!result.Succeeded) {
      response.HasError = true;
      response.Error = $"An error occurred while reset password";
      return response;
    }

    return response;
  }



  public async Task<IEnumerable<AccountDto>> GetAll() {
    var accounts = _userManager.Users.AsEnumerable();
    var userRoles = async (ApplicationUser user) => await _userManager.GetRolesAsync(user);

    var query = from account in accounts
                select account.ToAccountDto(userRoles(account).Result.FirstOrDefault());

    return query;
  }

  public async Task<AccountDto> GetById(string id) {
    var account = await _userManager.FindByIdAsync(id);

    var query = account.ToAccountDto(await _userManager.GetRolesAsync(account).ContinueWith(t => t.Result.FirstOrDefault()));

    return query;
  }

  public async Task<RegisterResponse> UpdateUserAsync(RegisterRequest request) {
    RegisterResponse response = new() {
      HasError = false
    };

    var user = await _userManager.FindByIdAsync(request.Id);

    if (user == null) {
      response.HasError = true;
      response.Error = $"No Accounts registered with {request.Email}";
      return response;
    }

    var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
    if (userWithSameUserName != null && userWithSameUserName.Id != user.Id) {
      response.HasError = true;
      response.Error = $"username '{request.UserName}' is already taken.";
      return response;
    }

    var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
    if (userWithSameEmail != null && userWithSameEmail.Id != user.Id) {
      response.HasError = true;
      response.Error = $"Email '{request.Email}' is already taken.";
      return response;
    }

    if (!string.IsNullOrWhiteSpace(request.Password)) {
      var passwordValidator = new PasswordValidator<ApplicationUser>();
      var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, request.Password);

      if (!passwordValidationResult.Succeeded) {
        response.HasError = true;
        response.Error = passwordValidationResult.Errors.First().Description;
        return response;
      }

      user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
    }


    user.Email = request.Email;
    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.UserName = request.UserName;
    user.DNI = request.DNI;
    user.PhoneNumber = request.PhoneNumber;
    user.Image = request.Image;
    user.EmailConfirmed = true;
    user.PhoneNumberConfirmed = true;

    var result = await _userManager.UpdateAsync(user);

    if (!result.Succeeded) {
      response.HasError = true;
      response.Error = $"An error occurred while updating user";
      return response;
    }

    return response;
  }

  public async Task<SaveUserVm> GetEntity(string id) {
    var account = await _userManager.FindByIdAsync(id);
    var userRole = await _userManager.GetRolesAsync(account).ContinueWith(t => t.Result.FirstOrDefault());

    var role = 4;

    switch (userRole) {
      case "Admin":
        role = 1;
        break;
      case "Dev":
        role = 2;
        break;
      case "Agent":
        role = 3;
        break;
    }

    var query = account.ToSaveVm(role);

    return query;
  }
  public async Task ChangeStatus(string id) {
    var user = await _userManager.FindByIdAsync(id);

    user.EmailConfirmed = !user.EmailConfirmed;

    await _userManager.UpdateAsync(user);
  }

}




