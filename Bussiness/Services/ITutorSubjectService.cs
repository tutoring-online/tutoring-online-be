using DataAccess.Entities.TutorSubject;
using DataAccess.Models.TutorSubject;

namespace tutoring_online_be.Services;

public interface ITutorSubjectService
{
    IEnumerable<TutorSubjectDto> GetTutorSubjects();
    IEnumerable<TutorSubjectDto> GetTutorSubjectById(string id);
    void CreateTutorSubjects(TutorSubject tutorSubjects);
    void UpdateTutorSubjects(TutorSubject tutorSubject, string id);
    int DeleteTutorSubject(string id);
    Dictionary<string, TutorSubjectDto> GetTutorSubjects(HashSet<string> ids);


}