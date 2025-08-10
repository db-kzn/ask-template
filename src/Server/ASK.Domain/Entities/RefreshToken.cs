namespace ASK.Domain.Entities;

public class RefreshToken
{
  public string Token { get; set; } = null!;
  public string UserId { get; set; } = null!;
  public string TenantId { get; set; } = null!;
  public DateTime Expires { get; set; }
  public bool IsRevoked { get; set; }
  public DateTime CreatedOn { get; set; }
  public string? CreatedByIp { get; set; }
}
