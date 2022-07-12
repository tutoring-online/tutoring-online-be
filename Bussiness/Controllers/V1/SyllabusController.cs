using DataAccess.Entities.Subject;
using DataAccess.Entities.Syllabus;
using DataAccess.Models;
using DataAccess.Models.Subject;
using DataAccess.Models.Syllabus;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;


[ApiController]
[Route("/api/v1/syllabuses")]
public class SyllabusController : Controller
{
    private readonly ISyllabusService syllabusService;
    private readonly ISubjectService subjectService;

    public SyllabusController(
        ISyllabusService syllabusService,
        ISubjectService subjectService
        )
    {
        this.syllabusService = syllabusService;
        this.subjectService = subjectService;
    }

    [HttpGet]
    public IActionResult GetSyllabuses([FromQuery] PageRequestModel model, [FromQuery] SearchSyllabusRequest request)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(request))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Syllabus));
            Page<SearchSyllabusResponse> responseData = syllabusService.GetSyllabuses(model, orderByParams, request);

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<SearchSyllabusResponse> syllabusDtos = responseData.Data;
                HashSet<string> subjectIds = syllabusDtos.Select(t => t.SubjectId).NotEmpty().ToHashSet();
                
                if (subjectIds.Count > 0)
                {
                    Dictionary<string, SubjectDto> subjectDtos = subjectService.GetSubjects(subjectIds);
                    
                    foreach (var item in syllabusDtos.Where(item => subjectDtos.ContainsKey(item.SubjectId)))
                    {
                        item.Subject = subjectDtos[item.SubjectId];
                    }
                }
            }

            return Ok(responseData);
        }
            
        return Ok(syllabusService.GetSyllabuses());
    }
    

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<SyllabusDto> GetSyllabus(string id)
    {
        var syllabuses = syllabusService.GetSyllabusById(id);
        return syllabuses;
    }
    
    [HttpPost]
    public IActionResult CreateSyllabuses([FromBody]IEnumerable<CreateSyllabusDto> syllabusDto)
    {
        IEnumerable<Syllabus> syllabuses = syllabusDto.Select(syllabusDto => syllabusDto.AsEntity());

        syllabusService.CreateSyllabuses(syllabuses);
        return Created("", "");
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateSyllabus(string id, [FromBody]UpdateSyllabusDto updateSyllabusDto)
    {
        var syllabuses = syllabusService.GetSyllabusById(id);
        if (syllabuses.Any())
        {
            syllabusService.UpdateSyllabus(updateSyllabusDto.AsEntity(), id);
        }
    }

}
