using DataAccess.Entities.Lesson;
using DataAccess.Entities.Payment;
using DataAccess.Entities.Subject;
using DataAccess.Entities.Syllabus;
using DataAccess.Models.Lesson;
using DataAccess.Models.Payment;
using DataAccess.Models.Subject;
using DataAccess.Models.Syllabus;

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

    public static SyllabusDto AsDtoSyllabuses(this Syllabus syllabus)
    {
        return new SyllabusDto()
        {
            Id = syllabus.Id,
            SubjectId = StringUtils.NullToEmpty(syllabus.SubjectId),
            TotalLessons = syllabus.TotalLessons,
            Description = StringUtils.NullToEmpty(syllabus.Description),
            Name = StringUtils.NullToEmpty(syllabus.Name),
            Status = syllabus.Status,
            UpdatedDate = StringUtils.NullToEmpty(syllabus.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(syllabus.CreatedDate),
            Price = syllabus.Price
        };
    }
    public static Syllabus AsEntity(this SyllabusDto syllabusDto)
    {
        return new Syllabus()
        {
            Id = syllabusDto.Id,
            SubjectId = StringUtils.NullToEmpty(syllabusDto.SubjectId),
            TotalLessons = syllabusDto.TotalLessons,
            Description = StringUtils.NullToEmpty(syllabusDto.Description),
            Name = StringUtils.NullToEmpty(syllabusDto.Name),
            Status = syllabusDto.Status,
            UpdatedDate = StringUtils.NullToEmpty(syllabusDto.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(syllabusDto.CreatedDate),
            Price = syllabusDto.Price
        };
    }

    public static PaymentDto AsDtoPayments(this Payment payment)
    {
        return new PaymentDto()
        {
            Id = payment.Id,
            SyllabusId = StringUtils.NullToEmpty(payment.SyllabusId),
            StudentId = StringUtils.NullToEmpty(payment.StudentId),
            Status = payment.Status,
            UpdatedDate = StringUtils.NullToEmpty(payment.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(payment.CreatedDate)
        };
    }

    public static Payment AsEntity(this PaymentDto paymentDto)
    {
        return new Payment()
        {
            Id = paymentDto.Id,
            SyllabusId = StringUtils.NullToEmpty(paymentDto.SyllabusId),
            StudentId = StringUtils.NullToEmpty(paymentDto.StudentId),
            Status = paymentDto.Status,
            UpdatedDate = StringUtils.NullToEmpty(paymentDto.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(paymentDto.CreatedDate)
        };
    }
}
