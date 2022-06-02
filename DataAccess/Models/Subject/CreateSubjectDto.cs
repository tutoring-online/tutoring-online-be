﻿namespace DataAccess.Models.Subject;

public class CreateSubjectDto
{
    public string? Code { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public int? Status { get; init; }
    
    public string? CategoryId { get; init; }
}