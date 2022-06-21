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
    
    public void UpdateTotalPages()
    {
        var totalPage = (int)(this.TotalItems / this.Size);
        if (totalPage < 1)
            totalPage = 1;
        else if (this.TotalItems % this.Size > 0)
            totalPage += 1;

        this.TotalPages = totalPage;
    }
}