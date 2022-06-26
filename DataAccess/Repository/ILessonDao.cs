using DataAccess.Entities.Lesson;
using DataAccess.Models;
using DataAccess.Models.Lesson;


namespace DataAccess.Repository;

public interface ILessonDao
{
    IEnumerable<Lesson?> GetLessons();

    IEnumerable<Lesson?> GetLessonById(string id);
        
    void CreateLessons(IEnumerable<Lesson> lessons);
    void UpdateLessons(Lesson lesson, string id);
    int DeleteLesson(string id);
    Page<Lesson> GetLessons(int? getLimit, int? getOffSet, List<Tuple<string, string>> orderByParams, SearchLessonRequest request, bool isNotPaging);
}

