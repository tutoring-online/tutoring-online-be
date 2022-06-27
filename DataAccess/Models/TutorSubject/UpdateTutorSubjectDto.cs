using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.TutorSubject;

public class UpdateTutorSubjectDto
{
    public string? Id { get; init; }
    public string? TutorId { get; init; }
    public string? SubjectId { get; init; }
    public int? Status { get; init; }
}
