using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class PageRequestModel : IValidatableObject
{
    public int? Page { get; set; }
    public int? Size { get; set; }
    public string? Sort { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        if (Page is not null)
        {
            if (Page <= 0)
                validationResults.Add(new ValidationResult($"{nameof(Page)} is equal or below zero"));

        }

        if (Size is not null)
        {
            if (Size < 0)
                validationResults.Add(new ValidationResult($"{nameof(Size)} is equal or below zero"));
        }

        if (Sort is not null)
        {
            
        }

        return validationResults;
    }
}