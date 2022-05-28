﻿
using DataAccess.Models.Subject;

namespace tutoring_online_be.Services;

public interface ISubjectService
{
    IEnumerable<SubjectDto> GetSubjects();
    IEnumerable<SubjectDto> GetSubjectById(string id);
}