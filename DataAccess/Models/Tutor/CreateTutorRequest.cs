namespace DataAccess.Models.Tutor;

public class CreateTutorDto
{
    public string Email { get; init; }
    public int[]? Subjects { get; init; }

}