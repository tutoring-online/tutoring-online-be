using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.TutorSubject;

public class TutorSubject
{
    public string? Id { get; init; }
    public string? TutorId { get; init; }
    public string? SubjectId { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? Status { get; init; }
}
