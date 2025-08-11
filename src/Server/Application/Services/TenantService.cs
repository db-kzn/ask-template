namespace ASK.Server.Application.Services;

public class TenantService : ITenantService
{
  private static readonly AsyncLocal<string?> _currentTenantId = new();

  public string? GetTenantId() => _currentTenantId.Value;

  public void SetTenantId(string? tenantId) => _currentTenantId.Value = tenantId;
}
