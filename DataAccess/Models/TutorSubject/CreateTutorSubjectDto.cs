namespace DataAccess.Models.TutorSubject;

public class CreateTutorSubjectDto
{
    public string? TutorId { get; init; }
    public string? SubjectId { get; init; }
    public int? Status { get; init; }
}