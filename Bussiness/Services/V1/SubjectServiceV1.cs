
using DataAccess.Entities.Subject;
using DataAccess.Models;
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

    public void CreateSubjects(IEnumerable<Subject> subjects)
    {
        subjectDao.CreateSubjects(subjects);
    }

    public void UpdateSubjects(Subject subject, string id)
    {
        subjectDao.UpdateSubjects(subject, id);
    }

    public int DeleteSubject(string id)
    {
        return subjectDao.DeleteSubject(id);
    }

    public Page<SearchSubjectResponse> GetSubjects(PageRequestModel model, List<Tuple<string, string>> orderByParams, SearchSubjectRequest request)
    {
        Page<Subject> result = new Page<Subject>();
        Page<SearchSubjectResponse> resultDto = new Page<SearchSubjectResponse>();

        result = subjectDao.GetSubjects(model.GetLimit(), model.GetOffSet(), orderByParams, request, model.IsNotPaging());

        resultDto.Data = result.Data.Select(p => p.AsSearchDto()).ToList();
        resultDto.Pagination = result.Pagination;
        resultDto.Pagination.Page = model.GetPage();
        resultDto.Pagination.Size = model.GetSize();
        if (model.IsNotPaging())
        {
            resultDto.Pagination.TotalPages = 1;
            resultDto.Pagination.Size = resultDto.Pagination.TotalItems;
        }

        if (!result.Data.Any())
            resultDto.Pagination.TotalPages = 0;
        else
            resultDto.Pagination.UpdateTotalPages();
        
        return resultDto;
    }

    public Dictionary<string, SubjectDto> GetSubjects(HashSet<string> subjectIds)
    {
        return subjectDao.GetSubjects(subjectIds).ToDictionary(pair => pair.Key, pair => pair.Value.AsDto());
    }
}