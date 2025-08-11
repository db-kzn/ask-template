using ASK.Server.Application.Services;

namespace ASK.Server.Api.Middleware;

public class TenantMiddleware
{
  private readonly RequestDelegate _next;

  public TenantMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
  {
    var tenantId = context.User.Claims
        .FirstOrDefault(c => c.Type == "tenant_id")?.Value;

    if (!string.IsNullOrEmpty(tenantId))
    {
      tenantService.SetTenantId(tenantId);
    }

    await _next(context);
  }
}
