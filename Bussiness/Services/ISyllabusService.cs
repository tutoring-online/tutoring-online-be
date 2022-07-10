using DataAccess.Entities.Syllabus;
using DataAccess.Models;
using DataAccess.Models.Syllabus;

namespace tutoring_online_be.Services;

public interface ISyllabusService
{
    IEnumerable<SyllabusDto> GetSyllabuses();

    IEnumerable<SyllabusDto> GetSyllabusById(string id);

    void CreateSyllabuses(IEnumerable<Syllabus> syllabuses);
    void UpdateSyllabus(Syllabus asEntity, string id);
    Dictionary<string, SyllabusDto> GetSyllabuses(HashSet<string> ids);
    Page<SearchSyllabusResponse> GetSyllabuses(PageRequestModel page, List<Tuple<string, string>> orderByParams, SearchSyllabusRequest request);
}
