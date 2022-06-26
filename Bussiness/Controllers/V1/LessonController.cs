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

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<SearchLessonReponse> data = responseData.Data;
                HashSet<string> studentIds = data.Select(t => t.StudentId).NotEmpty().ToHashSet();
                HashSet<string> syllabusIds = data.Select(t => t.SyllabusId).NotEmpty().ToHashSet();
                HashSet<string> tutorIds = data.Select(t => t.TutorId).NotEmpty().ToHashSet();

                
                if (studentIds.Count > 0)
                {
                    Dictionary<string, StudentDto> studentDtos = studentService.GetStudents(studentIds);
                    
                    foreach (var item in data.Where(item => studentDtos.ContainsKey(item.StudentId)))
                    {
                        item.Student = studentDtos[item.StudentId];
                    }
                }

                if (syllabusIds.Count > 0)
                {
                    Dictionary<string, SyllabusDto> syllabusDtos = syllabusService.GetSyllabuses(syllabusIds);
                
                    foreach (var item in data.Where(item => syllabusDtos.ContainsKey(item.SyllabusId)))
                    {
                        item.Syllabus = syllabusDtos[item.SyllabusId];
                    }
                }

                if (tutorIds.Count() > 0)
                {
                    Dictionary<string, TutorDto> tutorDtos = tutorService.GetTutors(tutorIds);
                    
                    foreach (var item in data.Where(item => tutorDtos.ContainsKey(item.SyllabusId)))
                    {
                        item.Tutor = tutorDtos[item.SyllabusId];
                    }
                }
            }
            
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
    public void CreateLessons(IEnumerable<CreateLessonDto> lessonDto)
    {
        IEnumerable<Lesson> lessons = lessonDto.Select(lessonDto => lessonDto.AsEntity());

        lessonService.CreateLessons(lessons);

    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateLesson(string id, UpdateLessonDto lessonDto)
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

