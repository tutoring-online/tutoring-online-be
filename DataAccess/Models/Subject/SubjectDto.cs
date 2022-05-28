namespace DataAccess.Models.Subject;

public class SubjectDto
{
    public string Id { get; init; }

    public string? SubjectCode { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public string? Status { get; init; }
    
    public string? CreatedDate { get; init; }
    
    public string? UpdatedDate { get; init; }
    
    public string? CategoryId { get; init; }
}