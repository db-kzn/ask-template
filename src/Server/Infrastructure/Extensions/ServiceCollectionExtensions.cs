using ASK.Server.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASK.Server.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>((sp, options) =>
    {
      var connectionString = configuration.GetConnectionString("DefaultConnection")
          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

      options.UseNpgsql(connectionString, npgsqlOptions =>
      {
        npgsqlOptions.MigrationsAssembly("ASK.Server.Infrastructure");
        npgsqlOptions.EnableRetryOnFailure();
      });
    });

    return services;
  }
}
