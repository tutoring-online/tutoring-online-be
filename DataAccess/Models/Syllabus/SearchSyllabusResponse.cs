using System.Text.Json.Serialization;
using DataAccess.Models.Subject;

namespace DataAccess.Models.Syllabus;

public class SearchSyllabusResponse
{
    public string? Id { get; init; }
    
    [JsonIgnore]
    public string? SubjectId { get; init; }
    public SubjectDto? Subject { get; set; }
    public int? TotalLessons { get; init; }
    public string? Description { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
    public double? Price { get; init; }
    public int? Status { get; init; }
    public string? Name { get; init; }
}