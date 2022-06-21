using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;

namespace DataAccess.Models.Payment;

public class PaymentDto
{
    public string? Id { get; init; }
    
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public string? CreatedDate { get; init; }
    
    public string? UpdatedDate { get; init; }
    
    public int? Status { get; init; }
    
    public StudentDto? Student { get; set; }
    
    public SyllabusDto? Syllabus { get; set; }
}
