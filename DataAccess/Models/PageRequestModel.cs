using System.ComponentModel.DataAnnotations;
using System.Net;
using Anotar.NLog;

namespace DataAccess.Models;

public class PageRequestModel : IValidatableObject
{
    public int? Page { get; set; }
    public int? Size { get; set; }
    public string? Sort { get; set; }

    public bool IsNotPaging()
    {
        return this.Size is null || this.Size == 0;
    }

    public int? GetSize()
    {
        return this.Size switch
        {
            null => 10,
            > 0 => this.Size,
            _ => 0
        };
    }

    public int? GetLimit()
    {
        return this.GetSize();
    }

    public int? GetOffSet()
    {
        return this.GetSize() * this.GetPage();
    }

    public int? GetPage()
    {
        return this.Page is null ? 0 : this.Page;
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> validationResults = new List<ValidationResult>();
        if (Page is not null)
        {
            if (Page < 0)
                validationResults.Add(new ValidationResult($"{nameof(Page)} is below zero"));

        }

        if (Size is not null)
        {
            if (Size < 0)
                validationResults.Add(new ValidationResult($"{nameof(Size)} is below zero"));
        }

        return validationResults;
    }
}