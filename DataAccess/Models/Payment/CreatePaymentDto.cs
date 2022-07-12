namespace DataAccess.Models.Payment;

public class CreatePaymentDto
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public int? Status { get; init; }
    
    public int? Combo { get; init; }
    
    public int? DateSession { get; init; }
    
}