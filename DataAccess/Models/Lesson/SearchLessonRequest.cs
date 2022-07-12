using System.Text.Json.Serialization;

namespace DataAccess.Models.Lesson;

public class SearchLessonRequest
{
    
    public int?[]? SlotNumber { get; init; }
    
    public DateTime? FromDate { get; init; }
    
    public DateTime? ToDate { get; init; }
    
    public DateTime? FromCreatedDate { get; init; }
    
    public DateTime? ToCreatedDate { get; init; }
    
    public DateTime? FromUpdatedDate { get; init; }
    
    public DateTime? ToUpdatedDate { get; init; }
    
    public int? Status { get; init; }
    
    public string? PaymentId { get; init; }
    
}