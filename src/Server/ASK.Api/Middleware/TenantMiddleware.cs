using ASK.Application.Services;

namespace ASK.Api.Middleware;

public class TenantMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ITenantService _tenantService;

  public TenantMiddleware(RequestDelegate next, ITenantService tenantService)
  {
    _next = next;
    _tenantService = tenantService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var tenantId = context.User.Claims
        .FirstOrDefault(c => c.Type == "tenant_id")?.Value;

    if (!string.IsNullOrEmpty(tenantId))
    {
      _tenantService.SetTenantId(tenantId);
    }

    await _next(context);
  }
}
