using System.ComponentModel.DataAnnotations;

namespace RSApp.Core.Services.ViewModels.SaveVm;

public class SavePersonVm {
  public string? Id { get; set; } = null!;

  [Required(ErrorMessage = "Name is required")]
  public string FirstName { get; set; } = null!;
  [Required(ErrorMessage = "Last Name is required")]
  public string LastName { get; set; } = null!;
  [Required(ErrorMessage = "Email is required")]
  public string Email { get; set; } = null!;
  [Required(ErrorMessage = "PhoneNumber is required")]
  public string PhoneNumber { get; set; } = null!;

  public string DNI { get; set; } = null!;

  public bool HasError { get; set; } = false;
  public string? Error { get; set; } = null!;

}
