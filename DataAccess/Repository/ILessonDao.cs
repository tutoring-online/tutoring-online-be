using DataAccess.Entities.Lesson;


namespace DataAccess.Repository;

public interface ILessonDao
{
    IEnumerable<Lesson?> GetLessons();

    IEnumerable<Lesson?> GetLessonById(string id);
        
    void CreateLessons(IEnumerable<Lesson> lessons);
    void UpdateLessons(Lesson lesson, string id);
}

