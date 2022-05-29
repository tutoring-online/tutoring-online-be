using DataAccess.Entities.Lesson;
using DataAccess.Entities.Subject;
using DataAccess.Models.Lesson;
using DataAccess.Models.Subject;

namespace DataAccess.Utils;

public static class Extensions
{
    public static SubjectDto AsDto(this Subject subject)
    {
        return new SubjectDto()
        {
            Id = subject.Id,
            Name = StringUtils.NullToEmpty(subject.Name),
            Description = StringUtils.NullToEmpty(subject.Description),
            Status = subject.Status,
            CategoryId = StringUtils.NullToEmpty(subject.CategoryId),
            SubjectCode = StringUtils.NullToEmpty(subject.SubjectCode),
            UpdatedDate = StringUtils.NullToEmpty(subject.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(subject.CreatedDate)
        };
    }
    
    public static Subject AsEntity(this SubjectDto subjectDto)
    {
        return new Subject()
        {
            Id = subjectDto.Id,
            Name = StringUtils.NullToEmpty(subjectDto.Name),
            Description = StringUtils.NullToEmpty(subjectDto.Description),
            Status = subjectDto.Status,
            CategoryId = StringUtils.NullToEmpty(subjectDto.CategoryId),
            SubjectCode = StringUtils.NullToEmpty(subjectDto.SubjectCode),
            UpdatedDate = StringUtils.NullToEmpty(subjectDto.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(subjectDto.CreatedDate)
        };
    }

    public static LessonDto AsDtoLesson(this Lesson lesson)
    {
        return new LessonDto()
        {
            Id = lesson.Id,
            SyllabusId = StringUtils.NullToEmpty(lesson.SyllabusId),
            TutorId = StringUtils.NullToEmpty(lesson.TutorId),
            StudentId = StringUtils.NullToEmpty(lesson.StudentId),
            Status = lesson.Status,
            Date = StringUtils.NullToEmpty(lesson.Date),
            UpdatedDate = StringUtils.NullToEmpty(lesson.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(lesson.CreatedDate),
            SlotNumer = lesson.SlotNumer
        };
    }
    public static Lesson AsEntity(this LessonDto lessonDto)
    {
        return new Lesson()
        {
            Id = lessonDto.Id,
            SyllabusId = StringUtils.NullToEmpty(lessonDto.SyllabusId),
            TutorId = StringUtils.NullToEmpty(lessonDto.TutorId),
            StudentId = StringUtils.NullToEmpty(lessonDto.StudentId),
            Status = lessonDto.Status,
            Date = StringUtils.NullToEmpty(lessonDto.Date),
            UpdatedDate = StringUtils.NullToEmpty(lessonDto.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(lessonDto.CreatedDate),
            SlotNumer = lessonDto.SlotNumer
        };
    }
}
