namespace ASK.Shared.Dto;

public record TenantDto(
    string Id,
    string Name,
    string? ConnectionString,
    string? Schema,
    bool IsActive);
