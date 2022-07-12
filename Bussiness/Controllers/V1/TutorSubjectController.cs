using DataAccess.Models.TutorSubject;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/tutor-subjects")]
public class TutorSubjectController
{
    private readonly ITutorSubjectService tutorSubjectService;

    public TutorSubjectController(ITutorSubjectService tutorSubjectService)
    {
        this.tutorSubjectService = tutorSubjectService;
    }

    [HttpGet]
    public IEnumerable<TutorSubjectDto> GetTutorSubjects()
    {
        return tutorSubjectService.GetTutorSubjects();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<TutorSubjectDto> GetTutorSubject(string id)
    {
        var tutorSubjects = tutorSubjectService.GetTutorSubjectById(id);

        return tutorSubjects;
    }

    [HttpPatch]
    [Route("{id}")]
    public void GetTutorSubject(string id, [FromBody]UpdateTutorSubjectDto updateTutorSubjectDto)
    {
        var tutorSubjects = tutorSubjectService.GetTutorSubjectById(id);
        if (tutorSubjects.Any())
        {
            tutorSubjectService.UpdateTutorSubjects(updateTutorSubjectDto.AsEntity(), id);
        }

    }

    [HttpDelete]
    [Route("{id}")]
    public void DeleteTutorSubject(string id)
    {
        tutorSubjectService.DeleteTutorSubject(id);
    }
}