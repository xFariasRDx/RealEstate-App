using RSApp.Core.Services.Dtos.Account;
using RSApp.Core.Services.Helpers;

namespace RSApp.Presentation.WebApp.Middleware;

public class ValidateSessions {
  private readonly IHttpContextAccessor _httpContextAccessor;

  public ValidateSessions(IHttpContextAccessor httpContextAccessor) {
    _httpContextAccessor = httpContextAccessor;
  }

  public bool HasUser() {
    AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
    return userViewModel != null;
  }
  public bool IsAdmin() {
    AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
    return userViewModel != null && userViewModel.Roles.Where(x => x.ToString() == "Admin").Any();
  }
}
