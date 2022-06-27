using DataAccess.Entities.TutorSubject;

namespace DataAccess.Repository;

public interface ITutorSubjectDao
{
    IEnumerable<TutorSubject?> GetTutorSubjects();
    Dictionary<string, TutorSubject?> GetTutorSubjects(HashSet<string> ids);
    IEnumerable<TutorSubject?> GetTutorSubjectById(string id);
    int CreateTutorSubject(TutorSubject tutorSubject);
    void UpdateTutorSubject(TutorSubject asEntity, string id);
    int DeleteTutorSubject(string id);
}