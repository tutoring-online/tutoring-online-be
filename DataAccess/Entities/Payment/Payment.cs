using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Payment;

public class Payment
{
    public string? Id { get; init; }
    public string? SyllabusId { get; init; }
    public string? StudentId { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
    public int? Status { get; init; }
}
