
using DataAccess.Entities.Subject;
using DataAccess.Models.Subject;

namespace DataAccess.Repository;

public interface ISubjectDao
{
    IEnumerable<Subject?> GetSubjects();
    
    IEnumerable<Subject?> GetSubjectById(string id);

    void CreateSubjects(IEnumerable<Subject> subjects);

    void UpdateSubjects(Subject subject);
}