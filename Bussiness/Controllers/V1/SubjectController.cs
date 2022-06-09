using System.Collections;
using Anotar.NLog;
using DataAccess.Entities.Subject;
using DataAccess.Models.Subject;
using DataAccess.Utils;
using Microsoft.AspNetCore.Authorization;
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
    [Route("{id}")]
    public IEnumerable<SubjectDto> GetSubject(string id)
    {
        var subjects= subjectService.GetSubjectById(id);
        return subjects;
    }
    
    [HttpPost]
    public void CreateSubjects(IEnumerable<CreateSubjectDto> subjectDto)
    {
        IEnumerable<Subject> subjects = subjectDto.Select(subjectDto => subjectDto.AsEntity());
        
        subjectService.CreateSubjects(subjects);
      
    }
    

}
