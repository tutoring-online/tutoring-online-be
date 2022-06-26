namespace DataAccess.Models.Subject;

public class SearchSubjectRequest
{
    public string? Code { get; init; }
    
    public string? Name { get; init; }
    
    public DateTime? FromCreatedDate { get; init; }
    
    public DateTime? ToCreatedDate { get; init; }
    
    public DateTime? FromUpdatedDate { get; init; }
    
    public DateTime? ToUpdatedDate { get; init; }
    
    public int? Status { get; init; }

    public int?[]? CategoryId { get; init; }
}