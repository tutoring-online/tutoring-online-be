using DataAccess.Entities.Lesson;
using DataAccess.Models.Lesson;

namespace tutoring_online_be.Services;

public interface ILessonService
{
        IEnumerable<LessonDto> GetLessons();

        IEnumerable<LessonDto> GetLessonById(string id);

        void CreateLessons(IEnumerable<Lesson> lessons);
}

