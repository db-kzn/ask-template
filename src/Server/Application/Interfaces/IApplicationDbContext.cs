using ASK.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASK.Server.Application.Interfaces;

public interface IApplicationDbContext
{
  DbSet<ApplicationUser> Users { get; }

  DbSet<RefreshToken> RefreshTokens { get; }

  DbSet<Tenant> Tenants { get; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
