
using DataAccess.Models.Subject;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class SubjectServiceV1 : ISubjectService
{
    private readonly ISubjectDao subjectDao;
    
    public SubjectServiceV1(ISubjectDao subjectDao)
    {
        this.subjectDao = subjectDao;
    }

    public IEnumerable<SubjectDto> GetSubjects()
    {
        return subjectDao.GetSubjects().Select(subject => subject.AsDto());
    }

    public IEnumerable<SubjectDto> GetSubjectById(string id)
    {
        return subjectDao.GetSubjectById(id).Select(subject => subject.AsDto());
    }
}