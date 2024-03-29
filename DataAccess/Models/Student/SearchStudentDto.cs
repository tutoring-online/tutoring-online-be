﻿namespace DataAccess.Models.Student;

public class SearchStudentDto
{
    public string? Email { get; init; }
    public string? Name { get; init; }
    public int? Grade { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; init; }
    public int? Gender { get; init; }
    public string? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
}