using ASK.Application.Interfaces;
using ASK.Application.Services;
using ASK.Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASK.Application.Features.Auth;

public record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress) : ICommand<LoginResult>;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, LoginResult>
{
  private readonly IApplicationDbContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly TokenService _tokenService;
  private readonly ITenantService _tenantService;

  public RefreshTokenCommandHandler(
      IApplicationDbContext db,
      UserManager<ApplicationUser> userManager,
      TokenService tokenService,
      ITenantService tenantService)
  {
    _db = db;
    _userManager = userManager;
    _tokenService = tokenService;
    _tenantService = tenantService;
  }

  public async ValueTask<LoginResult> Handle(RefreshTokenCommand command, CancellationToken ct)
  {
    var storedToken = await _db.RefreshTokens
        .FirstOrDefaultAsync(t => t.Token == command.RefreshToken, ct);

    if (storedToken == null || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
      throw new InvalidOperationException("Invalid refresh token.");

    var user = await _userManager.FindByIdAsync(storedToken.UserId);
    if (user == null)
      throw new InvalidOperationException("User not found.");

    _tenantService.SetTenantId(user.TenantId);

    var roles = await _userManager.GetRolesAsync(user);
    var newAccessToken = _tokenService.GenerateJwt(user, roles);
    var newRefreshToken = _tokenService.GenerateRefreshToken();

    // Отзываем старый токен
    storedToken.IsRevoked = true;
    _db.RefreshTokens.Update(storedToken);

    // Добавляем новый
    var newTokenEntity = new RefreshToken
    {
      Token = newRefreshToken,
      UserId = user.Id,
      TenantId = user.TenantId,
      Expires = DateTime.UtcNow.AddDays(7),
      CreatedOn = DateTime.UtcNow,
      CreatedByIp = command.IpAddress
    };

    await _db.RefreshTokens.AddAsync(newTokenEntity, ct);
    await _db.SaveChangesAsync(ct);

    return new LoginResult(newAccessToken, newRefreshToken, newTokenEntity.Expires);
  }
}
