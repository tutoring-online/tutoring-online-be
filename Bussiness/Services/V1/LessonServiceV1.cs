using DataAccess.Entities.Lesson;
using DataAccess.Models;
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
        return lessonDao.GetLessons().Select(lesson => lesson.AsDto());
    }

    public IEnumerable<LessonDto> GetLessonById(string id)
    {
        return lessonDao.GetLessonById(id).Select(lesson => lesson.AsDto());
    }

    public void CreateLessons(IEnumerable<Lesson> lessons)
    {
        lessonDao.CreateLessons(lessons);
    }

    public void UpdateLessons(Lesson lesson, string id)
    {
        lessonDao.UpdateLessons(lesson, id);
    }

    public int DeleteLesson(string id)
    {
        return lessonDao.DeleteLesson(id);
    }

    public Page<SearchLessonReponse> GetLessons(PageRequestModel model, List<Tuple<string, string>> orderByParams, SearchLessonRequest request)
    {
        Page<Lesson> result = new Page<Lesson>();
        Page<SearchLessonReponse> resultDto = new Page<SearchLessonReponse>();

        result = lessonDao.GetLessons(model.GetLimit(), model.GetOffSet(), orderByParams, request, model.IsNotPaging());

        resultDto.Data = result.Data.Select(p => p.AsSearchDto()).ToList();
        resultDto.Pagination = result.Pagination;
        resultDto.Pagination.Page = model.GetPage();
        resultDto.Pagination.Size = model.GetSize();
        if (model.IsNotPaging())
        {
            resultDto.Pagination.TotalPages = 1;
            resultDto.Pagination.Size = resultDto.Pagination.TotalItems;
        }

        if (!result.Data.Any())
            resultDto.Pagination.TotalPages = 0;
        else
            resultDto.Pagination.UpdateTotalPages();
        
        return resultDto;
    }
}

