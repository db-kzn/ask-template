using Microsoft.Extensions.DependencyInjection;

namespace ASK.Application;

public static class Extensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddMediator(options =>
    {
      options.ServiceLifetime = ServiceLifetime.Scoped;
      options.Namespace = "ASK";
    });

    return services;
  }
}
