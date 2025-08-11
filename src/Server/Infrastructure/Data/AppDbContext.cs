using ASK.Server.Application.Interfaces;
using ASK.Server.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASK.Server.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<string>, string>, IApplicationDbContext
{
  private readonly string? _tenantId;

  public AppDbContext(DbContextOptions<AppDbContext> options, string? tenantId = null) : base(options)
  {
    _tenantId = tenantId;
  }

  public DbSet<Tenant> Tenants => Set<Tenant>();

  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Настройка связи ApplicationUser → Tenant
    builder.Entity<ApplicationUser>()
        .HasOne<Tenant>()
        .WithMany()
        .HasForeignKey(u => u.TenantId)
        .OnDelete(DeleteBehavior.Cascade);

    // Global Query Filter для мультитенантности
    builder.Entity<ApplicationUser>()
        .HasQueryFilter(u => u.TenantId == _tenantId);

    // Настройка RefreshToken
    builder.Entity<RefreshToken>(entity =>
    {
      entity.HasKey(rt => rt.Token);
      entity.HasIndex(rt => rt.UserId);
      entity.HasIndex(rt => rt.Expires);
    });

    // Убираем префиксы "AspNet"
    foreach (var entityType in builder.Model.GetEntityTypes())
    {
      var tableName = entityType.GetTableName();
      if (tableName!.StartsWith("AspNet"))
      {
        entityType.SetTableName(tableName[6..]);
      }
    }
  }
}
