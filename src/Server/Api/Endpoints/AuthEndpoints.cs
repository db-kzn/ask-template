using ASK.Server.Application.Features.Auth;
using ASK.Shared.Dto;
using Mediator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ASK.Server.Api.Endpoints;

public static class AuthEndpoints
{
  public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
  {
    var group = app.MapGroup("/api/auth").WithTags("Auth");

    group.MapPost("/login", async (LoginRequest request, ISender sender) =>
    {
      var command = new LoginCommand(
          request.Email,
          request.Password,
          request.TenantId,
          request.IpAddress);

      var result = await sender.Send(command);

      var response = new AuthResponse(
          result.AccessToken,
          result.RefreshToken,
          result.ExpiresOn);

      return Results.Ok(response);
    })
    .WithName("Login")
    .WithDescription("Authenticate user and return tokens")
    .WithOpenApi();

    group.MapPost("/refresh", async (RefreshTokenRequest request, ISender sender) =>
    {
      var command = new RefreshTokenCommand(request.RefreshToken, request.IpAddress);
      var result = await sender.Send(command);

      var response = new AuthResponse(
          result.AccessToken,
          result.RefreshToken,
          result.ExpiresOn);

      return Results.Ok(response);
    })
    .WithName("RefreshToken")
    .WithDescription("Generate new access token using refresh token")
    .WithOpenApi();
  }
}
