using System.Collections;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Models.Subject;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/subjects")]
public class SubjectController
{
    private readonly ISubjectService subjectService;
    
    public SubjectController(ISubjectService subjectService)
    {
        this.subjectService = subjectService;
    }

    [HttpGet]
    public IEnumerable<SubjectDto> GetSubjects()
    {
        return subjectService.GetSubjects();
    }
}