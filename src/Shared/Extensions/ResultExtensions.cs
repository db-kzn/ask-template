using ASK.Shared.Common;
using Error = ASK.Shared.Common.Error;

namespace ASK.Shared.Extensions;

public static class ResultExtensions
{
  public static Result<T> ToResult<T>(this T? value, Error error = default)
  {
    if (value is null)
      return Result.Failure<T>(error ?? Error.NullValue);

    return Result.Success(value);
  }

  public static Result<T> Validate<T>(this T value, Func<T, bool> predicate, Error error)
  {
    return predicate(value) ? Result.Success(value) : Result.Failure<T>(error);
  }
}
