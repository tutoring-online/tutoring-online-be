namespace DataAccess.Entities.Payment;

public class Payment
{
    public string? Id { get; init; }
    public string? SyllabusId { get; init; }
    public string? StudentId { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? Status { get; init; }
    
    public int? Combo { get; init; }
    
    public int? DateSession { get; init; }
    
    public DateTime? StartDate { get; init; }
    
    public DateTime? EndDate { get; init; }
    
    public string? TutorId { get; init; }
}
