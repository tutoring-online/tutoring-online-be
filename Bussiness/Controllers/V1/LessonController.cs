using DataAccess.Entities.Lesson;
using DataAccess.Models;
using DataAccess.Models.Lesson;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Models.Tutor;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/lessons")]
public class LessonController : Controller
{
    private readonly ILessonService lessonService;
    private readonly IStudentService studentService;
    private readonly ISyllabusService syllabusService;
    private readonly ITutorService tutorService;

    public LessonController(
        ILessonService lessonService,
        IStudentService studentService,
        ITutorService tutorService,
        ISyllabusService syllabusService
        )
    {
        this.lessonService = lessonService;
        this.studentService = studentService;
        this.tutorService = tutorService;
        this.syllabusService = syllabusService;
    }

    [HttpGet]
    public IActionResult GetLessons([FromQuery]PageRequestModel model, [FromQuery]SearchLessonRequest request)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(request))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Lesson));

            Page<SearchLessonReponse> responseData = lessonService.GetLessons(model, orderByParams, request);

            return Ok(responseData);
            
        }
        
        return Ok(lessonService.GetLessons());
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<LessonDto> GetLesson(string id)
    {
        var lessons = lessonService.GetLessonById(id);

        return lessons;
    }
    [HttpPost]
    public IActionResult CreateLessons([FromBody]IEnumerable<CreateLessonDto> lessonDto)
    {
        IEnumerable<Lesson> lessons = lessonDto.Select(lessonDto => lessonDto.AsEntity());

        lessonService.CreateLessons(lessons);
        return Created("", "");
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateLesson(string id, [FromBody]UpdateLessonDto lessonDto)
    {
        var lessons = lessonService.GetLessonById(id);
        if (lessons.Any())
        {
            lessonService.UpdateLessons(lessonDto.AsEntity(), id);
        }

    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeleteLesson(string id)
    {
        lessonService.DeleteLesson(id);
    }
}

