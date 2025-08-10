using Microsoft.AspNetCore.Identity;

namespace ASK.Domain.Entities;

public class ApplicationUser : IdentityUser<string>
{
  public string TenantId { get; set; } = null!;
  public DateTime CreatedOn { get; set; }
  public string? CreatedBy { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public bool IsActive { get; set; } = true;
}
