namespace DataAccess.Models.Payment;

public class UpdatePaymentDto
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public int? Status { get; init; }
}