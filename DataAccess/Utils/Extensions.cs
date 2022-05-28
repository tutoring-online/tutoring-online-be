using DataAccess.Entities.Subject;
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
}
