using DataAccess.Entities.Lesson;
using DataAccess.Models.Lesson;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/lessons")]
public class LessonController 
{
    private readonly ILessonService lessonService;

    public LessonController(ILessonService lessonService)
    {
        this.lessonService = lessonService;
    }

    [HttpGet]
    public IEnumerable<LessonDto> GetLessons()
    {
        return lessonService.GetLessons();
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
}

