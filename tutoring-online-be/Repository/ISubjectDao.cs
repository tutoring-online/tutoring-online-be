using tutoring_online_be.Entities.Subject;

namespace tutoring_online_be.Repository;

public interface ISubjectDao
{
    public IEnumerable<Subject?> GetSubjects();
}