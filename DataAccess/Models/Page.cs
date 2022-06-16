namespace DataAccess.Models;

public class Page
{
    public IEnumerable<object> Data { get; set; }
    
    public PageDetail Pagination { get; set; }
}

public class PageDetail
{
    public int Items { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}