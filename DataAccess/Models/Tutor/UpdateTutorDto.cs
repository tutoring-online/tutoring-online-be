namespace DataAccess.Models.Tutor;

public class UpdateTutorDto
{
    public string? Email { get; init; }
    public string? Name { get; init; }
    public string? MeetingUrl { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; init; }
    public int? Gender { get; init; }
    public string? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
    public string? Description { get; init; }
}