﻿namespace DataAccess.Models.Subject;

public class SubjectDto
{
    public string? Id { get; init; }

    public string? Code { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public int? Status { get; init; }
    
    public string? CreatedDate { get; set; }
    
    public string? UpdatedDate { get; init; }
    
    public string? CategoryId { get; init; }
}