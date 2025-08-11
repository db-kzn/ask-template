namespace ASK.Server.Domain.Entities;

public class TenantDetail
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? ConnectionString { get; set; }
  public string? Schema { get; set; }
  public bool IsActive { get; set; }
  public DateTime CreatedOn { get; set; }
  public string? CreatedBy { get; set; }
}
