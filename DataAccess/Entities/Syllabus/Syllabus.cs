namespace DataAccess.Entities.Syllabus;

public class Syllabus
{
    public string? Id { get; init; }
    public string? SubjectId { get; init; }
    public int? TotalLessons { get; init; }
    public string? Description { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
    public double? Price { get; init; }
    public int? Status { get; init; }
    public string? Name { get; init; }
}
