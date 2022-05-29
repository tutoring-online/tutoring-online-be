namespace DataAccess.Entities.Lesson
{
    public class Lesson
    {
        public string? Id { get; init; }
        public string? SyllabusId { get; init; }
        public string? TutorId { get; init; }
        public string? StudentId { get; init; }
        public int? SlotNumer { get; init; }
        public string? Date { get; init; }
        public string? CreatedDate { get; init; }
        public string? UpdatedDate { get; init; }
        public int? Status { get; init; }
    }
}
