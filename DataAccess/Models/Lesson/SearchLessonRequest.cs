﻿using System.Text.Json.Serialization;

namespace DataAccess.Models.Lesson;

public class SearchLessonRequest
{
    public string? SyllabusId { get; init; }
    
    public string? StudentId { get; init; }
    
    public string? TutorId { get; init; }
    
    public int?[]? SlotNumber { get; init; }
    
    public DateTime? FromDate { get; init; }
    
    public DateTime? ToDate { get; init; }
    
    public DateTime? FromCreatedDate { get; init; }
    
    public DateTime? ToCreatedDate { get; init; }
    
    public DateTime? FromUpdatedDate { get; init; }
    
    public DateTime? ToUpdatedDate { get; init; }
    
    public int? Status { get; init; }
    
}