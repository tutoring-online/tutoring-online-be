
using DataAccess.Entities.Subject;

namespace DataAccess.Repository;

public interface ISubjectDao
{
    public IEnumerable<Subject?> GetSubjects();
}