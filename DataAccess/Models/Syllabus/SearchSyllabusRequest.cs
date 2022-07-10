namespace DataAccess.Models.Syllabus;

public class SearchSyllabusRequest
{
    public int[]? SubjectId { get; init; }
    public int? FromTotalLessons { get; init; }
    public int? ToTotalLessons { get; init; }
    public string? Description { get; init; }
    public DateTime? FromCreatedDate { get; init; }
    public DateTime? ToCreatedDate { get; init; }
    public DateTime? FromUpdatedDate { get; init; }
    public DateTime? ToUpdatedDate { get; init; }
    public double? FromPrice { get; init; }
    public double? ToPrice { get; init; }
    public int? Status { get; init; }
    public string? Name { get; init; }
}