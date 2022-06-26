using DataAccess.Entities.Lesson;
using DataAccess.Models;
using DataAccess.Models.Lesson;

namespace tutoring_online_be.Services;

public interface ILessonService
{
        IEnumerable<LessonDto> GetLessons();

        IEnumerable<LessonDto> GetLessonById(string id);

        void CreateLessons(IEnumerable<Lesson> lessons);
        
        void UpdateLessons(Lesson lesson, string id);
        int DeleteLesson(string id);
        Page<SearchLessonReponse> GetLessons(PageRequestModel model, List<Tuple<string, string>> orderByParams, SearchLessonRequest request);
}

