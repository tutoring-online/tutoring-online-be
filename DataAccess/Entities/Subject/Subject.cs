namespace DataAccess.Entities.Subject;

public class Subject
{
    public string? Id { get; init; }

    public string? Code { get; set; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public int? Status { get; init; }
    
    public DateTime? CreatedDate { get; set; }
    
    public DateTime? UpdatedDate { get; init; }
    
    public string? CategoryId { get; init; }
    
    
}