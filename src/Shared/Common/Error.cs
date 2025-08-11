namespace ASK.Shared.Common;

public record Error(string Code, string Message)
{
  public static readonly Error None = new(string.Empty, string.Empty);
  public static readonly Error NullValue = new("Error.NullValue", "Value cannot be null.");
  public static readonly Error ValidationError = new("Error.Validation", "One or more validation errors occurred.");
  public static readonly Error Unauthorized = new("Error.Unauthorized", "You are not authorized.");
  public static readonly Error Forbidden = new("Error.Forbidden", "You do not have permission.");
  public static readonly Error NotFound = new("Error.NotFound", "The requested resource was not found.");
}
