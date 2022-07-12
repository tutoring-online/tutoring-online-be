namespace DataAccess.Models.Lesson;

public class UpdateLessonDto
{
    
    public string? PaymentId { get; init; }
    public int? SlotNumber { get; init; }
    public string? Date { get; init; }
    public int? Status { get; init; }
}