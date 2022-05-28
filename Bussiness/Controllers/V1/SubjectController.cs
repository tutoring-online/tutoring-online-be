using System.Collections;
using DataAccess.Models.Subject;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

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