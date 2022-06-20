namespace DataAccess.Models;

public class Page<T>
{
    public List<T?> Data { get; set; }
    
    public PageDetail Pagination { get; set; }
}

public class PageDetail
{
    public int? Size { get; set; }
    public int? Page { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalItems { get; set; }
}