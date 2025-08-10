namespace ASK.Shared.Dto;

public record UserDto
(
  string Id,
  string Email,
  string? FirstName,
  string? LastName,
  string TenantId,
  DateTime CreatedOn
);
