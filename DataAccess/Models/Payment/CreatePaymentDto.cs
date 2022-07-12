namespace DataAccess.Models.Payment;

public class CreatePaymentDto
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public int? Status { get; init; }
    
}