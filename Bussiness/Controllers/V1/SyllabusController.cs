using DataAccess.Entities.Syllabus;
using DataAccess.Models.Syllabus;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;


[ApiController]
[Route("/api/v1/syllabuses")]
public class SyllabusController
{
    private readonly ISyllabusService syllabusService;

    public SyllabusController(ISyllabusService syllabusService)
    {
        this.syllabusService = syllabusService;
    }

    [HttpGet]
    public IEnumerable<SyllabusDto> GetSyllabuses()
    {
        return syllabusService.GetSyllabuses();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<SyllabusDto> GetSyllabus(string id)
    {
        var syllabuses = syllabusService.GetSyllabusById(id);
        return syllabuses;
    }
    
    [HttpPost]
    public void CreateSyllabuses(IEnumerable<CreateSyllabusDto> syllabusDto)
    {
        IEnumerable<Syllabus> syllabuses = syllabusDto.Select(syllabusDto => syllabusDto.AsEntity());

        syllabusService.CreateSyllabuses(syllabuses);

    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateSyllabus(string id, UpdateSyllabusDto updateSyllabusDto)
    {
        var syllabuses = syllabusService.GetSyllabusById(id);
        if (syllabuses.Any())
        {
            syllabusService.UpdateSyllabus(updateSyllabusDto.AsEntity(), id);
        }
    }

}
