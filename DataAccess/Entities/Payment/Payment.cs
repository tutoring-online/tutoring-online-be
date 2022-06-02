namespace DataAccess.Entities.Payment;

public class Payment
{
    public string? Id { get; init; }
    public string? SyllabusId { get; init; }
    public string? StudentId { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? Status { get; init; }
}
