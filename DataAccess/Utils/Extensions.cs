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
    
    //Subject
    public static SubjectDto AsDto(this Subject subject)
    {
        return new SubjectDto()
        {
            Id = subject.Id,
            Name = StringUtils.NullToEmpty(subject.Name),
            Description = StringUtils.NullToEmpty(subject.Description),
            Status = subject.Status,
            CategoryId = StringUtils.NullToEmpty(subject.CategoryId),
            Code = StringUtils.NullToEmpty(subject.Code),
            UpdatedDate = StringUtils.NullToEmpty(subject.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(subject.CreatedDate)
        };
    }
    
    public static Subject AsEntity(this SubjectDto subjectDto)
    {
        return new Subject()
        {
            Id = subjectDto.Id,
            Name = subjectDto.Name,
            Description = subjectDto.Description,
            Status = subjectDto.Status,
            CategoryId = subjectDto.CategoryId,
            Code = subjectDto.Code,
            UpdatedDate =subjectDto.UpdatedDate,
            CreatedDate = subjectDto.CreatedDate
        };
    }
    
    //Lesson
    public static LessonDto AsDto(this Lesson lesson)
    {
        return new LessonDto()
        {
            Id = lesson.Id,
            SyllabusId = lesson.SyllabusId,
            TutorId = lesson.TutorId,
            StudentId = lesson.StudentId,
            Status = lesson.Status,
            Date = lesson.Date,
            UpdatedDate = lesson.UpdatedDate,
            CreatedDate = lesson.CreatedDate,
            SlotNumer = lesson.SlotNumer
        };
    }
    public static Lesson AsEntity(this LessonDto lessonDto)
    {
        return new Lesson()
        {
            Id = lessonDto.Id,
            SyllabusId = lessonDto.SyllabusId,
            TutorId = lessonDto.TutorId,
            StudentId = lessonDto.StudentId,
            Status = lessonDto.Status,
            Date = lessonDto.Date,
            UpdatedDate = lessonDto.UpdatedDate,
            CreatedDate = lessonDto.CreatedDate,
            SlotNumer = lessonDto.SlotNumer
        };
    }

    //Syllabus
    public static SyllabusDto AsDto(this Syllabus syllabus)
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
            SubjectId = syllabusDto.SubjectId,
            TotalLessons = syllabusDto.TotalLessons,
            Description = syllabusDto.Description,
            Name = syllabusDto.Name,
            Status = syllabusDto.Status,
            UpdatedDate = syllabusDto.UpdatedDate,
            CreatedDate = syllabusDto.CreatedDate,
            Price = syllabusDto.Price
        };
    }

    //Payment
    public static PaymentDto AsDto(this Payment payment)
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
            SyllabusId = paymentDto.SyllabusId,
            StudentId = paymentDto.StudentId,
            Status = paymentDto.Status,
            UpdatedDate = paymentDto.UpdatedDate,
            CreatedDate = paymentDto.CreatedDate
        };
    }
}
