namespace DataAccess.Entities.Syllabus;

public class Syllabus
{
    public string? Id { get; init; }
    public string? SubjectId { get; init; }
    public int? TotalLessons { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public double? Price { get; init; }
    public int? Status { get; init; }
    public string? Name { get; init; }
    
    public string? ImageUrl { get; init; }
    
    public string? VideoUrl { get; init; }
}
