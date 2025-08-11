namespace ASK.Server.Application.Services;

public interface ITenantService
{
  string? GetTenantId();
  void SetTenantId(string? tenantId);
}
