using System.Text.Json.Serialization;
using DataAccess.Models.Category;

namespace DataAccess.Models.Subject;

public class SearchSubjectResponse
{
    public string? Id { get; set; }

    public string? Code { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int? Status { get; set; }
    
    public string? CreatedDate { get; set; }
    
    public string? UpdatedDate { get; set; }
    
    [JsonIgnore]
    public string? CategoryId { get; set; }
    
    public CategoryDto? Category{ get; set; }
}