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
            UpdatedDate = CommonUtils.ConvertDateTimeToString(subject.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(subject.CreatedDate)
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
            UpdatedDate = CommonUtils.ConvertStringToDateTime(subjectDto.UpdatedDate),
            CreatedDate =  CommonUtils.ConvertStringToDateTime(subjectDto.CreatedDate)
        };
    }
    
    public static Subject AsEntity(this CreateSubjectDto subjectDto)
    {
        return new Subject()
        {
            Name = subjectDto.Name,
            Description = subjectDto.Description,
            Status = subjectDto.Status,
            CategoryId = subjectDto.CategoryId,
            Code = subjectDto.Code,
            CreatedDate = DateTime.Now
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
            Date = CommonUtils.ConvertDateTimeToString(lesson.Date),
            UpdatedDate = CommonUtils.ConvertDateTimeToString(lesson.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(lesson.CreatedDate),
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
            Date = CommonUtils.ConvertStringToDateTime(lessonDto.Date),
            UpdatedDate = CommonUtils.ConvertStringToDateTime(lessonDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(lessonDto.CreatedDate),
            SlotNumer = lessonDto.SlotNumer
        };
    }
    
    public static Lesson AsEntity(this CreateLessonDto lessonDto)
    {
        return new Lesson()
        {
            SyllabusId = lessonDto.SyllabusId,
            TutorId = lessonDto.TutorId,
            StudentId = lessonDto.StudentId,
            Status = lessonDto.Status,
            Date = CommonUtils.ConvertStringToDateTime(lessonDto.Date),
            CreatedDate = DateTime.Now
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
            UpdatedDate = CommonUtils.ConvertDateTimeToString(syllabus.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(syllabus.CreatedDate),
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
            UpdatedDate = CommonUtils.ConvertStringToDateTime(syllabusDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(syllabusDto.CreatedDate),
            Price = syllabusDto.Price
        };
    }
    
    public static Syllabus AsEntity(this CreateSyllabusDto syllabusDto)
    {
        return new Syllabus()
        {
            SubjectId = syllabusDto.SubjectId,
            TotalLessons = syllabusDto.TotalLessons,
            Description = syllabusDto.Description,
            Name = syllabusDto.Name,
            Status = syllabusDto.Status,
            CreatedDate = DateTime.Now,
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
            UpdatedDate = CommonUtils.ConvertDateTimeToString(payment.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(payment.CreatedDate)
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
            UpdatedDate = CommonUtils.ConvertStringToDateTime(paymentDto.CreatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(paymentDto.UpdatedDate)
        };
    }
    
    public static Payment AsEntity(this CreatePaymentDto paymentDto)
    {
        return new Payment()
        {
            SyllabusId = paymentDto.SyllabusId,
            StudentId = paymentDto.StudentId,
            Status = paymentDto.Status,
            CreatedDate = DateTime.Now
        };
    }
}
