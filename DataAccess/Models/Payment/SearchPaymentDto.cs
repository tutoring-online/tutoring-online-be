namespace DataAccess.Models.Payment;

public class SearchPaymentDto
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public DateTime? FromCreatedDate { get; init; }
    
    public DateTime? ToCreatedDate { get; init; }
    
    public DateTime? FromUpdatedDate { get; init; }
    
    public DateTime? ToUpdatedDate { get; init; }
    
    public int? Status { get; init; }
    
}