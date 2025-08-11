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
      var connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
          //throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

      if (string.IsNullOrEmpty(connectionString))
      {
        // Для разработки можно использовать SQLite
        connectionString = configuration.GetConnectionString("Sqlite") ?? "Data Source=ask.db";

        options.UseSqlite(connectionString, sqliteOptions =>
        {
          sqliteOptions.MigrationsAssembly("ASK.Server.Migrations.SQLite");
        });
      }
      else
      {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
          npgsqlOptions.MigrationsAssembly("ASK.Server.Migrations.PostgreSQL");
          npgsqlOptions.EnableRetryOnFailure();
        });
      }
    });

    return services;
  }
}
