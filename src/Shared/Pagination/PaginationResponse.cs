namespace ASK.Shared.Pagination;

public class PaginationResponse<T>
{
  public IReadOnlyCollection<T> Data { get; }
  public int Page { get; }
  public int PageSize { get; }
  public int TotalCount { get; }
  public int TotalPages { get; }
  public bool HasPrevious => Page > 1;
  public bool HasNext => Page < TotalPages;

  public PaginationResponse(IReadOnlyCollection<T> data, int totalCount, int page, int pageSize)
  {
    Data = data;
    Page = page;
    PageSize = pageSize;
    TotalCount = totalCount;
    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
  }
}
