namespace ASK.Server.Domain.Entities;

/// <summary>
/// Tenant
/// </summary>
public class Tenant
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string? ConnectionString { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedOn { get; set; }
  public string? CreatedBy { get; set; }
}
