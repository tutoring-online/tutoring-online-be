using System.Text.Json.Serialization;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Models.Tutor;

namespace DataAccess.Models.Lesson;

public class SearchLessonReponse
{
    public string? Id { get; init; }
    
    [JsonIgnore]
    public string? SyllabusId { get; set; }
    
    [JsonIgnore]
    public string? StudentId { get; set; }
    
    [JsonIgnore]
    public string? TutorId { get; set; }
    
    public int? SlotNumber { get; init; }

    public string? CreatedDate { get; set; }
    
    public string? UpdatedDate { get; set; }
    
    public int? Status { get; set; }
    
    public StudentDto? Student { get; set; }
    
    public SyllabusDto? Syllabus { get; set; }
    
    public TutorDto? Tutor { get; set; }
    
    public DateTime? Date { get; set; }

}