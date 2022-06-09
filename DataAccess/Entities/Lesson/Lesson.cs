namespace DataAccess.Entities.Lesson
{
    public class Lesson
    {
        public string? Id { get; init; }
        public string? SyllabusId { get; init; }
        public string? TutorId { get; init; }
        public string? StudentId { get; init; }
        public int? SlotNumber { get; init; }
        public DateTime? Date { get; init; }
        public DateTime? CreatedDate { get; init; }
        public DateTime? UpdatedDate { get; init; }
        public int? Status { get; init; }
    }
}
