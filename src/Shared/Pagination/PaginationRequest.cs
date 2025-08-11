namespace ASK.Shared.Pagination;

public class PaginationRequest
{
  public int Page { get; set; } = 1;
  public int PageSize { get; set; } = 10;

  public PaginationRequest() { }

  public PaginationRequest(int page, int pageSize)
  {
    Page = page < 1 ? 1 : page;
    PageSize = pageSize < 1 ? 10 : pageSize;
  }
}
