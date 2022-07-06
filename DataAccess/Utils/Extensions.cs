using DataAccess.Entities.Admin;
using DataAccess.Entities.Category;
using DataAccess.Entities.Lesson;
using DataAccess.Entities.Payment;
using DataAccess.Entities.Student;
using DataAccess.Entities.Subject;
using DataAccess.Entities.Syllabus;
using DataAccess.Entities.Token;
using DataAccess.Entities.Tutor;
using DataAccess.Entities.TutorSubject;
using DataAccess.Models.Admin;
using DataAccess.Models.Category;
using DataAccess.Models.Lesson;
using DataAccess.Models.Payment;
using DataAccess.Models.Student;
using DataAccess.Models.Subject;
using DataAccess.Models.Syllabus;
using DataAccess.Models.Token;
using DataAccess.Models.Tutor;
using DataAccess.Models.TutorSubject;

namespace DataAccess.Utils;

public static class Extensions
{
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(e => e != null).Select(e => e!);
    }
    
    public static IEnumerable<T> NotEmpty<T>(this IEnumerable<T?> enumerable) where T : class
    {
        return enumerable.Where(e => e is not "").Select(e => e!);
    }
    
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
    
    public static SearchSubjectResponse AsSearchDto(this Subject subject)
    {
        return new SearchSubjectResponse()
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

    public static Subject AsEntity(this UpdateSubjectDto subjectDto)
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
            SlotNumber = lesson.SlotNumber
        };
    }
    
    public static SearchLessonReponse AsSearchDto(this Lesson lesson)
    {
        return new SearchLessonReponse()
        {
            Id = lesson.Id,
            SyllabusId = StringUtils.NullToEmpty(lesson.SyllabusId),
            StudentId = StringUtils.NullToEmpty(lesson.StudentId),
            TutorId = StringUtils.NullToEmpty(lesson.TutorId),
            SlotNumber = lesson.SlotNumber,
            Status = lesson.Status,
            Date = lesson.Date,
            UpdatedDate = CommonUtils.ConvertDateTimeToString(lesson.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(lesson.CreatedDate)
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
            SlotNumber = lessonDto.SlotNumber
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
            SlotNumber = lessonDto.SlotNumber,
            Date = CommonUtils.ConvertStringToDateTime(lessonDto.Date),
            CreatedDate = DateTime.Now
        };
    }
    
    public static Lesson AsEntity(this UpdateLessonDto lessonDto)
    {
        return new Lesson()
        {
            SyllabusId = lessonDto.SyllabusId,
            TutorId = lessonDto.TutorId,
            StudentId = lessonDto.StudentId,
            Status = lessonDto.Status,
            SlotNumber = lessonDto.SlotNumber,
            Date = CommonUtils.ConvertStringToDateTime(lessonDto.Date),
            UpdatedDate = DateTime.Now
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
    
    public static Syllabus AsEntity(this UpdateSyllabusDto syllabusDto)
    {
        return new Syllabus()
        {
            SubjectId = syllabusDto.SubjectId,
            TotalLessons = syllabusDto.TotalLessons,
            Description = syllabusDto.Description,
            Name = syllabusDto.Name,
            Status = syllabusDto.Status,
            UpdatedDate = DateTime.Now,
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
    
    public static SearchPaymentDto AsSearchDto(this Payment payment)
    {
        return new SearchPaymentDto()
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
    
    public static Payment AsEntity(this UpdatePaymentDto paymentDto)
    {
        return new Payment()
        {
            SyllabusId = paymentDto.SyllabusId,
            StudentId = paymentDto.StudentId,
            Status = paymentDto.Status,
            UpdatedDate = DateTime.Now
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
    
    //RefreshToken
    public static RefreshTokenDto AsDto(this RefreshToken token)
    {
        return new RefreshTokenDto()
        {
            Id = token.Id,
            Token = token.Token,
            JwtId = token.JwtId,
            UserId = token.UserId,
            UserRole = token.UserRole,
            IsUsed = token.IsUsed,
            IsRevoked = token.IsRevoked,
            IssuedAt = token.IssuedAt,
            ExpiredAt = token.ExpiredAt
        };
    }

    public static RefreshToken AsEntity(this RefreshTokenDto token)
    {
        return new RefreshToken()
        {
            Id = token.Id,
            Token = token.Token,
            JwtId = token.JwtId,
            UserId = token.UserId,
            UserRole = token.UserRole,
            IsUsed = token.IsUsed,
            IsRevoked = token.IsRevoked,
            IssuedAt = token.IssuedAt,
            ExpiredAt = token.ExpiredAt
        };
    }

    //Tutor
    public static TutorDto AsDto(this Tutor tutor)
    {
        return new TutorDto()
        {
            Id = tutor.Id,
            Name = StringUtils.NullToEmpty(tutor.Name),
            Description = StringUtils.NullToEmpty(tutor.Description),
            Email = StringUtils.NullToEmpty(tutor.Email),
            Status = tutor.Status,
            MeetingUrl = StringUtils.NullToEmpty(tutor.MeetingUrl),
            Phone = StringUtils.NullToEmpty(tutor.Phone),
            Gender = tutor.Gender,
            AvatarURL = StringUtils.NullToEmpty(tutor.AvatarURL),
            Address = StringUtils.NullToEmpty(tutor.Address),
            Birthday = CommonUtils.ConvertDateTimeToString(tutor.Birthday),
            UpdatedDate = CommonUtils.ConvertDateTimeToString(tutor.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(tutor.CreatedDate)
        };
    }

    public static Tutor AsEntity(this TutorDto tutorDto)
    {
        return new Tutor()
        {
            Id = tutorDto.Id,
            Name = tutorDto.Name,
            Description = tutorDto.Description,
            Email = tutorDto.Email,
            Status = tutorDto.Status,
            MeetingUrl = tutorDto.MeetingUrl,
            Phone = tutorDto.Phone,
            Gender = tutorDto.Gender,
            AvatarURL = tutorDto.AvatarURL,
            Address = tutorDto.Address,
            Birthday = CommonUtils.ConvertStringToDateTime(tutorDto.Birthday),
            UpdatedDate = CommonUtils.ConvertStringToDateTime(tutorDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(tutorDto.CreatedDate)
        };
    }
    
    public static Tutor AsEntity(this UpdateTutorDto tutorDto)
    {
        return new Tutor()
        {
            Name = tutorDto.Name,
            Description = tutorDto.Description,
            Email = tutorDto.Email,
            Status = tutorDto.Status,
            MeetingUrl = tutorDto.MeetingUrl,
            Phone = tutorDto.Phone,
            Gender = tutorDto.Gender,
            AvatarURL = tutorDto.AvatarURL,
            Address = tutorDto.Address,
            Birthday = CommonUtils.ConvertStringToDateTime(tutorDto.Birthday),
            UpdatedDate = DateTime.Now
        };
    }

    //Student
    public static StudentDto AsDto(this Student student)
    {
        return new StudentDto()
        {
            Id = student.Id,
            Name = StringUtils.NullToEmpty(student.Name),         
            Email = StringUtils.NullToEmpty(student.Email),
            Status = student.Status,
            Grade = student.Grade,
            Phone = StringUtils.NullToEmpty(student.Phone),
            Gender = student.Gender,
            AvatarURL = StringUtils.NullToEmpty(student.AvatarURL),
            Address = StringUtils.NullToEmpty(student.Address),
            Birthday = CommonUtils.ConvertDateTimeToString(student.Birthday),
            UpdatedDate = CommonUtils.ConvertDateTimeToString(student.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(student.CreatedDate)
        };
    }

    public static Student AsEntity(this StudentDto studentDto)
    {
        return new Student()
        {
            Id = studentDto.Id,
            Name = studentDto.Name,         
            Email = studentDto.Email,
            Status = studentDto.Status,
            Grade = studentDto.Grade,
            Phone = studentDto.Phone,
            Gender = studentDto.Gender,
            AvatarURL = studentDto.AvatarURL,
            Address = studentDto.Address,
            Birthday = CommonUtils.ConvertStringToDateTime(studentDto.Birthday),
            UpdatedDate = CommonUtils.ConvertStringToDateTime(studentDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(studentDto.CreatedDate)
        };
    }
    
    public static Student AsEntity(this UpdateStudentDto studentDto)
    {
        return new Student()
        {
            Name = studentDto.Name,         
            Email = studentDto.Email,
            Status = studentDto.Status,
            Grade = studentDto.Grade,
            Phone = studentDto.Phone,
            Gender = studentDto.Gender,
            AvatarURL = studentDto.AvatarURL,
            Address = studentDto.Address,
            Birthday = CommonUtils.ConvertStringToDateTime(studentDto.Birthday),
            UpdatedDate = DateTime.Now,
        };
    }
    //Tutor-Subject
    public static TutorSubjectDto AsDto(this TutorSubject tutorSubject)
    {
        return new TutorSubjectDto()
        {
            Id = tutorSubject.Id,
            TutorId = tutorSubject.TutorId,
            SubjectId = tutorSubject.SubjectId,
            UpdatedDate = CommonUtils.ConvertDateTimeToString(tutorSubject.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(tutorSubject.CreatedDate),
            Status = tutorSubject.Status
            
        };
    }

    public static TutorSubject AsEntity(this TutorSubjectDto tutorSubjectDto)
    {
        return new TutorSubject()
        {
            Id = tutorSubjectDto.Id,
            TutorId = tutorSubjectDto.TutorId,
            SubjectId = tutorSubjectDto.SubjectId,
            UpdatedDate = CommonUtils.ConvertStringToDateTime(tutorSubjectDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(tutorSubjectDto.CreatedDate),
            Status = tutorSubjectDto.Status
        };
    }

    public static TutorSubject AsEntity(this UpdateTutorSubjectDto tutorSubjectDto)
    {
        return new TutorSubject()
        {
            Id = tutorSubjectDto.Id,
            TutorId = tutorSubjectDto.TutorId,
            SubjectId = tutorSubjectDto.SubjectId,
            UpdatedDate = DateTime.Now,
            Status = tutorSubjectDto.Status
        };
    }
    //Admin
    public static AdminDto AsDto(this Admin admin)
    {
        return new AdminDto()
        {
            Id = admin.Id,
            Name = StringUtils.NullToEmpty(admin.Name),
            Email = StringUtils.NullToEmpty(admin.Email),
            Status = admin.Status,
            Phone = StringUtils.NullToEmpty(admin.Phone),
            Gender = admin.Gender,
            AvatarURL = StringUtils.NullToEmpty(admin.AvatarURL),
            Address = StringUtils.NullToEmpty(admin.Address),
            Birthday = CommonUtils.ConvertDateTimeToString(admin.Birthday),
            UpdatedDate = CommonUtils.ConvertDateTimeToString(admin.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(admin.CreatedDate)
        };
    }

    public static Admin AsEntity(this AdminDto adminDto)
    {
        return new Admin()
        {
            Id = adminDto.Id,
            Name = adminDto.Name,
            Email = adminDto.Email,
            Status = adminDto.Status,
            Phone = adminDto.Phone,
            Gender = adminDto.Gender,
            AvatarURL = adminDto.AvatarURL,
            Address = adminDto.Address,
            Birthday = CommonUtils.ConvertStringToDateTime(adminDto.Birthday),
            UpdatedDate = CommonUtils.ConvertStringToDateTime(adminDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(adminDto.CreatedDate)
        };
    }
    
    public static Admin AsEntity(this UpdateAdminDto adminDto)
    {
        return new Admin()
        {
            Name = StringUtils.NullToEmpty(adminDto.Name),
            Email = StringUtils.NullToEmpty(adminDto.Email),
            Status = adminDto.Status,
            Phone = StringUtils.NullToEmpty(adminDto.Phone),
            Gender = adminDto.Gender,
            AvatarURL = StringUtils.NullToEmpty(adminDto.AvatarURL),
            Address = StringUtils.NullToEmpty(adminDto.Address),
            Birthday = CommonUtils.ConvertStringToDateTime(adminDto.Birthday),
            UpdatedDate = DateTime.Now,
        };
    }
    //Category
    public static CategoryDto AsDto(this Category category)
    {
        return new CategoryDto()
        {
            Id = category.Id,
            Description = StringUtils.NullToEmpty(category.Description),
            Name = StringUtils.NullToEmpty(category.Name),
            Type = category.Type,
            Status = category.Status,
            UpdatedDate = CommonUtils.ConvertDateTimeToString(category.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(category.CreatedDate)
           
        };
    }
    public static Category AsEntity(this CategoryDto categoryDto)
    {
        return new Category()
        {
            Id = categoryDto.Id,
            Description = StringUtils.NullToEmpty(categoryDto.Description),
            Name = StringUtils.NullToEmpty(categoryDto.Name),
            Type = categoryDto.Type,
            Status = categoryDto.Status,
            UpdatedDate = CommonUtils.ConvertStringToDateTime(categoryDto.UpdatedDate),
            CreatedDate = CommonUtils.ConvertStringToDateTime(categoryDto.CreatedDate)

        };
    }

    public static Category AsEntity(this UpdateCategoryDto categoryDto)
    {
        return new Category()
        {
            Description = StringUtils.NullToEmpty(categoryDto.Description),
            Name = StringUtils.NullToEmpty(categoryDto.Name),
            Type = categoryDto.Type,
            Status = categoryDto.Status,
            UpdatedDate = DateTime.Now
        };
    }

    public static Category AsEntity(this CreateCategoryDto categoryDto)
    {
        return new Category()
        {
            Description = StringUtils.NullToEmpty(categoryDto.Description),
            Name = StringUtils.NullToEmpty(categoryDto.Name),
            Type = categoryDto.Type,
            Status = categoryDto.Status,
            CreatedDate = DateTime.Now
        };
    }
    public static SearchCategoryDto AsSearchDto(this Category category)
    {
        return new SearchCategoryDto()
        {
            Id = category.Id,
            Description = StringUtils.NullToEmpty(category.Description),
            Name = StringUtils.NullToEmpty(category.Name),
            Type = category.Type,
            Status = category.Status,
            UpdatedDate = CommonUtils.ConvertDateTimeToString(category.UpdatedDate),
            CreatedDate = CommonUtils.ConvertDateTimeToString(category.CreatedDate)
            
        };
    }

}
