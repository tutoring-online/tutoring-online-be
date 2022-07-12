namespace DataAccess.Models.Payment;

public class UpdatePaymentDto
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public int? Status { get; init; }
    
    public int? Combo { get; init; }
    
    public int? DateSession { get; init; }
    
    public DateTime? StartDate { get; init; }
    
    public DateTime? EndDate { get; init; }
    
    public string? TutorId { get; init; }
}