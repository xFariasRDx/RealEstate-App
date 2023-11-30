using System.ComponentModel.DataAnnotations;

namespace RSApp.Core.Services.ViewModels.User;

public class ResetPasswordVm : ValidationVm {
  [Required(ErrorMessage = "The email is required")]
  [DataType(DataType.Text)]
  public string Email { get; set; } = null!;

  [Required(ErrorMessage = "You must provide a token")]
  [DataType(DataType.Text)]
  public string Token { get; set; } = null!;

  [Required(ErrorMessage = "You must provide a password")]
  [DataType(DataType.Password)]
  public string Password { get; set; } = null!;

  [Compare(nameof(Password), ErrorMessage = "The passwords do not match")]
  [Required(ErrorMessage = "You must confirm your password")]
  [DataType(DataType.Password)]
  public string ConfirmPassword { get; set; } = null!;
}
