using System.Text.Json.Serialization;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Models.Tutor;

namespace DataAccess.Models.Lesson;

public class SearchLessonReponse
{
    public string? Id { get; init; }
    
    public string? CreatedDate { get; set; }
    
    public string? UpdatedDate { get; set; }
    
    public int? Status { get; set; }
    
    public DateTime? Date { get; set; }
    
    public string? PaymentId { get; set; }

}