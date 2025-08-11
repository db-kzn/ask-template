using ASK.Server.Application.Interfaces;
using ASK.Server.Application.Services;
using ASK.Server.Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASK.Server.Application.Features.Auth;

public record LoginCommand(
    string Email,
    string Password,
    string TenantId,
    string IpAddress) : ICommand<LoginResult>;

public record LoginResult(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresOn);

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
{
  private readonly IApplicationDbContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly TokenService _tokenService;
  private readonly ITenantService _tenantService;

  public LoginCommandHandler(
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

  public async ValueTask<LoginResult> Handle(LoginCommand command, CancellationToken ct)
  {
    _tenantService.SetTenantId(command.TenantId);

    var user = await _userManager.FindByEmailAsync(command.Email);
    if (user == null || user.TenantId != command.TenantId)
      throw new InvalidOperationException("Invalid credentials.");

    var result = await _userManager.CheckPasswordAsync(user, command.Password);
    if (!result)
      throw new InvalidOperationException("Invalid credentials.");

    var roles = await _userManager.GetRolesAsync(user);

    var accessToken = _tokenService.GenerateJwt(user, roles);
    var refreshToken = _tokenService.GenerateRefreshToken();

    var existingToken = await _db.RefreshTokens
        .FirstOrDefaultAsync(t => t.UserId == user.Id && t.TenantId == command.TenantId, ct);

    if (existingToken != null)
    {
      existingToken.IsRevoked = true;
      _db.RefreshTokens.Update(existingToken);
    }

    var newRefreshToken = new RefreshToken
    {
      Token = refreshToken,
      UserId = user.Id,
      TenantId = user.TenantId,
      Expires = DateTime.UtcNow.AddDays(7),
      CreatedOn = DateTime.UtcNow,
      CreatedByIp = command.IpAddress
    };

    await _db.RefreshTokens.AddAsync(newRefreshToken, ct);
    await _db.SaveChangesAsync(ct);

    return new LoginResult(accessToken, refreshToken, newRefreshToken.Expires);
  }
}
