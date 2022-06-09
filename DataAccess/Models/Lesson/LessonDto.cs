using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Lesson;

public class LessonDto
{
    public string? Id { get; init; }
    public string? SyllabusId { get; init; }
    public string? TutorId { get; init; }
    public string? StudentId { get; init; }
    public int? SlotNumber { get; init; }
    public string? Date { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
    public int? Status { get; init; }
}
