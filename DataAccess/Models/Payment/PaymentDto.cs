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
    
    public int? Combo { get; init; }
    
    public int? DateSession { get; init; }
    
    public DateTime? StartDate { get; init; }
    
    public DateTime? EndDate { get; init; }
    

}
