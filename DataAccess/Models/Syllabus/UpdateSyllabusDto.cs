namespace DataAccess.Models.Syllabus;

public class UpdateSyllabusDto
{
    public string? SubjectId { get; init; }
    public int? TotalLessons { get; init; }
    public string? Description { get; init; }
    public double? Price { get; init; }
    public int? Status { get; init; }
    public string? Name { get; init; }
}