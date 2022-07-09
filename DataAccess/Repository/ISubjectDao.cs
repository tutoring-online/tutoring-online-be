
using DataAccess.Entities.Subject;
using DataAccess.Models;
using DataAccess.Models.Subject;

namespace DataAccess.Repository;

public interface ISubjectDao
{
    IEnumerable<Subject?> GetSubjects();
    
    IEnumerable<Subject?> GetSubjectById(string id);

    void CreateSubjects(IEnumerable<Subject> subjects);

    void UpdateSubjects(Subject subject, string id);
    int DeleteSubject(string id);
    Page<Subject> GetSubjects(int? getLimit, int? getOffSet, List<Tuple<string, string>> orderByParams, SearchSubjectRequest request, bool isNotPaging);
}