using tutoring_online_be.Models.Subject;

namespace tutoring_online_be.Services;

public interface ISubjectService
{
    IEnumerable<SubjectDto> GetSubjects();
}