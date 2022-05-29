using System.Collections;
using DataAccess.Entities.Subject;
using DataAccess.Models.Subject;
using DataAccess.Utils;
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

    [HttpGet]
    [Route("{id:int}")]
    public IEnumerable<SubjectDto> GetSubject(string id)
    {
        var subjects= subjectService.GetSubjectById(id);

        SubjectDto o = null;

        return subjects;
    }
    [HttpPost]
    public void CreateSubjects(IEnumerable<SubjectDto> subjectDto)
    {
        IEnumerable<Subject> subjects = subjectDto.Select(subjectDto => subjectDto.AsEntity());
        
        subjectService.CreateSubjects(subjects);
      
    }
    

}
