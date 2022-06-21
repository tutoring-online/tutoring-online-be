using DataAccess.Entities.Syllabus;
using DataAccess.Models.Syllabus;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class SyllabusServiceV1 : ISyllabusService
{
    private readonly ISyllabusDao syllabusDao;

    public SyllabusServiceV1(ISyllabusDao syllabusDao)
    {
        this.syllabusDao = syllabusDao;
    }

    public IEnumerable<SyllabusDto> GetSyllabuses()
    {
        return syllabusDao.GetSyllabuses().Select(syllabus => syllabus.AsDto());
    }

    public IEnumerable<SyllabusDto> GetSyllabusById(string id)
    {
        return syllabusDao.GetSyllabusById(id).Select(syllabus => syllabus.AsDto());
    }

    public void CreateSyllabuses(IEnumerable<Syllabus> syllabuses)
    {
        syllabusDao.CreateSyllabuses(syllabuses);
    }

    public void UpdateSyllabus(Syllabus asEntity, string id)
    {
        syllabusDao.UpdateSyllabus(asEntity, id);
    }

    public Dictionary<string, SyllabusDto> GetSyllabuses(HashSet<string> ids)
    {
        return syllabusDao.GetSyllabuses(ids).ToDictionary(pair => pair.Key, pair => pair.Value.AsDto());
    }
}
