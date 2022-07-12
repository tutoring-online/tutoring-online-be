namespace DataAccess.Entities.Lesson
{
    public class Lesson
    {
        public string? Id { get; init; }
        
        public string? PaymentId { get; init; }
        public int? SlotNumber { get; init; }
        public DateTime? Date { get; init; }
        public DateTime? CreatedDate { get; init; }
        public DateTime? UpdatedDate { get; init; }
        public int? Status { get; init; }
    }
}
