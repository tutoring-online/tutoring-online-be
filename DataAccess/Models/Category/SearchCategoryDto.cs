using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.Models.Category;

public class SearchCategoryDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }

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
    public int? Type { get; init; }
}
