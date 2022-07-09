
using DataAccess.Entities.Subject;
using DataAccess.Models;
using DataAccess.Models.Subject;

namespace tutoring_online_be.Services;

public interface ISubjectService
{
    IEnumerable<SubjectDto> GetSubjects();
    
    IEnumerable<SubjectDto> GetSubjectById(string id);

    void CreateSubjects(IEnumerable<Subject> subjects);
    void UpdateSubjects(Subject subject, string id);
    int DeleteSubject(string id);

    Page<SearchSubjectResponse> GetSubjects(PageRequestModel model, List<Tuple<string, string>> orderByParams, SearchSubjectRequest request);
}