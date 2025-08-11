using ASK.Server.Domain.Entities;
using ASK.Server.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ASK.Server.Infrastructure.Extensions;

public static class SeedData
{
  public static async Task SeedAsync(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();

    // Убедимся, что БД создана
    await context.Database.EnsureCreatedAsync();

    // 1. Сидирование ролей
    var roles = new[] { "admin", "moderator", "user" };
    foreach (var roleName in roles)
    {
      if (!await roleManager.RoleExistsAsync(roleName))
      {
        await roleManager.CreateAsync(new IdentityRole<string>(roleName));
      }
    }

    // 2. Сидирование тенантов
    if (!await context.Tenants.AnyAsync())
    {
      context.Tenants.AddRange(
          new Tenant
          {
            Id = "main",
            Name = "Main",
            IsActive = true,
            CreatedOn = DateTime.UtcNow
          },
          new Tenant
          {
            Id = "kosmosfit",
            Name = "KosmosFit",
            IsActive = true,
            CreatedOn = DateTime.UtcNow
          }
      );
      await context.SaveChangesAsync();
    }

    // 3. Сидирование пользователей
    var usersData = new[]
    {
            (Email: "admin@ask.local", FirstName: "Admin", LastName: "User", Role: "admin", TenantId: "main"),
            (Email: "instructor@ask.local", FirstName: "Instructor", LastName: "User", Role: "moderator", TenantId: "kosmosfit"),
            (Email: "client@ask.local", FirstName: "Client", LastName: "User", Role: "user", TenantId: "kosmosfit")
        };

    foreach (var (email, firstName, lastName, role, tenantId) in usersData)
    {
      if (await userManager.FindByEmailAsync(email) == null)
      {
        var user = new ApplicationUser
        {
          UserName = email,
          Email = email,
          FirstName = firstName,
          LastName = lastName,
          TenantId = tenantId,
          CreatedOn = DateTime.UtcNow,
          EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "P@ssw0rd!");
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(user, role);
        }
      }
    }
  }
}
