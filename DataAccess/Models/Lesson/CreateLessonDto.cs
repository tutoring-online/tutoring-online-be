namespace DataAccess.Models.Lesson;

public class CreateLessonDto
{
    public string? SyllabusId { get; init; }
    public string? TutorId { get; init; }
    public string? StudentId { get; init; }
    
    public string? SlotNumer { get; init; }
    public string? Date { get; init; }
    public int? Status { get; init; }
}