using DataAccess.Entities.Lesson;
using DataAccess.Models.Lesson;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class LessonServiceV1: ILessonService
{
    private readonly ILessonDao lessonDao;

    public LessonServiceV1(ILessonDao lessonDao)
    {
        this.lessonDao = lessonDao;
    }

    public IEnumerable<LessonDto> GetLessons()
    {
        return lessonDao.GetLessons().Select(lesson => lesson.AsDtoLesson());
    }

    public IEnumerable<LessonDto> GetLessonById(string id)
    {
        return lessonDao.GetLessonById(id).Select(lesson => lesson.AsDtoLesson());
    }

    public void CreateLessons(IEnumerable<Lesson> lessons)
    {
        lessonDao.CreateLessons(lessons);
    }
}

