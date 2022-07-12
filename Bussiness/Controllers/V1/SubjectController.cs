using System.Collections;
using Anotar.NLog;
using DataAccess.Entities.Subject;
using DataAccess.Models;
using DataAccess.Models.Category;
using DataAccess.Models.Subject;
using DataAccess.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/subjects")]
public class SubjectController : Controller
{
    private readonly ISubjectService subjectService;
    private readonly ICategoryService categoryService;
    
    public SubjectController(
        ISubjectService subjectService,
        ICategoryService categoryService
        )
    {
        this.subjectService = subjectService;
        this.categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetSubjects([FromQuery]PageRequestModel model, [FromQuery]SearchSubjectRequest request)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(request))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Subject));
            Page<SearchSubjectResponse> responseData = subjectService.GetSubjects(model, orderByParams, request);

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<SearchSubjectResponse> data = responseData.Data;
                HashSet<string> categoryIds = data.Select(t => t.CategoryId).NotEmpty().ToHashSet();

                
                if (categoryIds.Count > 0)
                {
                    Dictionary<string, CategoryDto> categoryDtos = categoryService.GetCategories(categoryIds);
                    
                    foreach (var item in data.Where(item => categoryDtos.ContainsKey(item.CategoryId)))
                    {
                        item.Category = categoryDtos[item.CategoryId];
                    }
                }

                return Ok(responseData);
            }
        }
        
        return Ok(subjectService.GetSubjects());
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<SubjectDto> GetSubject(string id)
    {
        var subjects= subjectService.GetSubjectById(id);
        return subjects;
    }
    
    [HttpPost]
    public IActionResult CreateSubjects([FromBody]IEnumerable<CreateSubjectDto> subjectDto)
    {
        IEnumerable<Subject> subjects = subjectDto.Select(subjectDto => subjectDto.AsEntity());
        
        subjectService.CreateSubjects(subjects);
        return Created("", "");
    }
    [HttpPatch]
    [Route("{id}")]
    public void UpdateSubject(string id, [FromBody]UpdateSubjectDto updateSubjectDto)
    {
        var subjects = subjectService.GetSubjectById(id);
        if (subjects.Any())
        {
            subjectService.UpdateSubjects(updateSubjectDto.AsEntity(), id);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public void DeleteSubject(string id)
    {
        subjectService.DeleteSubject(id);
    }


}
