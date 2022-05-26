using tutoring_online_be.Entities.Subject;
using tutoring_online_be.Models.Subject;

namespace tutoring_online_be.Utils;

public static class Extensions
{
    public static SubjectDto AsDto(this Subject subject)
    {
        return new SubjectDto()
        {
            Id = subject.Id,
            Name = StringUtils.NullToEmpty(subject.Name),
            Description = StringUtils.NullToEmpty(subject.Description),
            Status = StringUtils.NullToEmpty(subject.Status),
            CategoryId = StringUtils.NullToEmpty(subject.CategoryId),
            SubjectCode = StringUtils.NullToEmpty(subject.SubjectCode),
            UpdatedDate = StringUtils.NullToEmpty(subject.UpdatedDate),
            CreatedDate = StringUtils.NullToEmpty(subject.CreatedDate)
        };
    }
}
