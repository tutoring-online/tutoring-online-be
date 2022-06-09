using DataAccess.Models.Tutor;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/tutors")]
public class TutorController
{
    private readonly ITutorService tutorService;

    public TutorController(ITutorService tutorService)
    {
        this.tutorService = tutorService;
    }

    [HttpGet]
    public IEnumerable<TutorDto> GetTutors()
    {
        return tutorService.GetTutors();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<TutorDto> GetTutor(string id)
    {
        var tutors = tutorService.GetTutorById(id);

        return tutors;
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void GetTutor(string id, UpdateTutorDto updateTutorDto)
    {
        var tutors = tutorService.GetTutorById(id);
        if (tutors.Any())
        {
            tutorService.UpdateTutor(updateTutorDto.AsEntity(), id);
        }

    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeleteTutor(string id)
    {
            tutorService.DeleteTutor(id);
    }
}
