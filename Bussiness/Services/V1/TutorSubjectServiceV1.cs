using DataAccess.Entities.TutorSubject;
using DataAccess.Models.TutorSubject;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class TutorSubjectServiceV1:ITutorSubjectService
{
    private readonly ITutorSubjectDao tutorSubjectDao;

    public TutorSubjectServiceV1(ITutorSubjectDao tutorSubjectDao)
    {
        this.tutorSubjectDao = tutorSubjectDao;
    }
    public IEnumerable<TutorSubjectDto> GetTutorSubjects()
    {
        return tutorSubjectDao.GetTutorSubjects().Select(tutorSubject => tutorSubject.AsDto());
    }

    public IEnumerable<TutorSubjectDto> GetTutorSubjectById(string id)
    {
        return tutorSubjectDao.GetTutorSubjectById(id).Select(tutorSubject => tutorSubject.AsDto());
    }
    public void CreateTutorSubjects(TutorSubject tutorSubjects)
    {
        tutorSubjectDao.CreateTutorSubject(tutorSubjects);
    }

    public void UpdateTutorSubjects(TutorSubject tutorSubject, string id)
    {
        tutorSubjectDao.UpdateTutorSubject(tutorSubject, id);
    }

    public int DeleteTutorSubject(string id)
    {
        return tutorSubjectDao.DeleteTutorSubject(id);
    }
    public Dictionary<string, TutorSubjectDto> GetTutorSubjects(HashSet<string> ids)
    {
        return tutorSubjectDao.GetTutorSubjects(ids).ToDictionary(pair => pair.Key, pair => pair.Value.AsDto());
    }
}
