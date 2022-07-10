namespace DataAccess.Models.Student;

public class UpdateStudentDto
{
    public string? Name { get; init; }
    public int? Grade { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; init; }
    public int? Gender { get; init; }
    public string? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
}