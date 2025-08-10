namespace ASK.Application.Services;

public interface ITenantService
{
  string? GetTenantId();
  void SetTenantId(string? tenantId);
}
