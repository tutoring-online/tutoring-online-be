using System.Text.Json.Serialization;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;

namespace DataAccess.Models.Payment;

public class SearchPaymentDto
{
    public string? Id { get; init; }
    
    [JsonIgnore]
    public string? SyllabusId { get; init; }
    
    [JsonIgnore]
    public string? StudentId { get; init; }
    
    [JsonIgnore]
    public DateTime? FromCreatedDate { get; init; }
    
    [JsonIgnore]
    public DateTime? ToCreatedDate { get; init; }
    
    [JsonIgnore]
    public DateTime? FromUpdatedDate { get; init; }
    
    [JsonIgnore]
    public DateTime? ToUpdatedDate { get; init; }
    
    public string? CreatedDate { get; init; }
    
    public string? UpdatedDate { get; init; }
    
    public int? Status { get; init; }
    
    public StudentDto? Student { get; set; }
    
    public SyllabusDto? Syllabus { get; set; }
    
    public int? Combo { get; init; }
    
    public int? DateSession { get; init; }
    
    public DateTime? StartDate { get; init; }
    
    public DateTime? EndDate { get; init; }

}