using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class PageRequestModel : IValidatableObject
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public int? MaxItems { get; set; }
    public string? SortBy { get; set; }
    public string? Order { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        if (PageNumber is not null)
        {
            if (PageNumber <= 0)
                validationResults.Add(new ValidationResult($"{nameof(PageNumber)} is equal or below zero"));

        }

        if (PageSize is not null)
        {
            if (PageSize <= 0)
                validationResults.Add(new ValidationResult($"{nameof(PageSize)} is equal or below zero"));
        }

        return validationResults;
    }
}