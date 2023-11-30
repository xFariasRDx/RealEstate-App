namespace RSApp.Core.Services.Dtos;

public class EmailRequest {
  public string To { get; set; } = null!;
  public string? Subject { get; set; } = null!;
  public string Body { get; set; } = null!;
}

