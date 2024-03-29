﻿using Anotar.NLog;
using DataAccess.Entities.Token;
using DataAccess.Entities.TutorSubject;
using DataAccess.Models;
using DataAccess.Models.Subject;
using DataAccess.Models.Tutor;
using DataAccess.Models.TutorSubject;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/tutors")]
public class TutorController : Controller
{
    private readonly ITutorService tutorService;
    private readonly ITutorSubjectService tutorSubjectService;

    public TutorController(
        ITutorService tutorService,
        ITutorSubjectService tutorSubjectService
        )
    {
        this.tutorService = tutorService;
        this.tutorSubjectService = tutorSubjectService;
    }

    [HttpGet]
    public IEnumerable<TutorDto> GetTutors()
    {
        return tutorService.GetTutors();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<TutorDto> GetTutor(string id)
    {
        var tutors = tutorService.GetTutorById(id);
        if (tutors.Any())
        {
            TutorDto tutor = tutors.ElementAt(0);
            string tutorId = tutor.Id;
            IEnumerable<TutorSubjectDto> subjectDtos = tutorSubjectService.GetTutorSubjectsByTutorId(tutorId);

            tutor.Subjects = subjectDtos.Select(t => int.Parse(t.SubjectId)).ToHashSet().ToArray();

            return new List<TutorDto>
            {
                tutor
            };
        }

        return tutors;
    }

    [HttpPost]
    public IActionResult CreateTutor([FromBody]CreateTutorDto dto)
    {
        TutorDto tmp = tutorService.GetTutorByEmail(dto.Email);
        if (tmp is null)
        {
            LogTo.Info($"\nDo create tutor with email: {dto.Email}");
            tutorService.CreateTutor(dto);
            TutorDto tutorDto = tutorService.GetTutorByEmail(dto.Email);
            LogTo.Info($"\nCreate Tutor Done with email: {tutorDto.Email}, id: {tutorDto.Id}");
            if (dto.Subjects is not null && dto.Subjects.Any())
            {
                LogTo.Info($"\n Do create tutor subjects with id : {string.Join(",", dto.Subjects)}");
                List<TutorSubjectDto> tutorSubjectDtos = new List<TutorSubjectDto>();
                HashSet<int> subjectIds = dto.Subjects.ToHashSet();
            
                foreach (int subjectId in subjectIds)
                {
                    tutorSubjectDtos.Add(new TutorSubjectDto()
                    {
                        SubjectId = subjectId.ToString(),
                        TutorId = tutorDto.Id,
                        Status = (int)TutorSubjectStatus.Active
                    });
                }
                tutorSubjectService.CreateTutorSubjects(tutorSubjectDtos.Select(t => t.AsEntity()));

                TutorDto tutor = tutorService.GetTutorByEmail(dto.Email);
                IEnumerable<TutorSubjectDto> subjectDtos = tutorSubjectService.GetTutorSubjectsByTutorId(tutorDto.Id);

                tutor.Subjects = subjectDtos.Select(t => int.Parse(t.SubjectId)).ToHashSet().ToArray();

                return Created(new Uri($"api/v1/tutors/{tutorDto.Id}", UriKind.Relative), tutor);
            }
        }

        return BadRequest(new ApiResponse
            {
                ResultCode = (int)ResultCode.UserAlreadyCreated,
                ResultMessage = ResultCode.UserAlreadyCreated.ToString()
        });
        
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateTutor(string id, [FromBody]UpdateTutorDto updateTutorDto)
    {
        var tutors = tutorService.GetTutorById(id);
        if (tutors.Any())
        {
            tutorService.UpdateTutor(updateTutorDto.AsEntity(), id);

            if (updateTutorDto.Subjects is not null && updateTutorDto.Subjects.Any())
            {
                LogTo.Info($"\nDo Update Tutor Subjects with ids : {string.Join(",", updateTutorDto.Subjects)}");
                LogTo.Info($"\nDo Remove all tutor subject with tutor id : {id}");
                
                int result = tutorSubjectService.DeleteTutorSubjectsByTutorId(id);
                if (result != -1)
                {
                    LogTo.Info($"\nDo Create Tutor Subjects");
                    List<TutorSubjectDto> tutorSubjectDtos = new List<TutorSubjectDto>();
                    HashSet<int> subjectIds = updateTutorDto.Subjects.ToHashSet();
            
                    foreach (int subjectId in subjectIds)
                    {
                        tutorSubjectDtos.Add(new TutorSubjectDto()
                        {
                            SubjectId = subjectId.ToString(),
                            TutorId = tutors.ElementAt(0).Id,
                            Status = (int)TutorSubjectStatus.Active
                        });
                    }
                    tutorSubjectService.CreateTutorSubjects(tutorSubjectDtos.Select(t => t.AsEntity()));
                }
            }
        }

    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeleteTutor(string id)
    {
            tutorService.DeleteTutor(id);
    }
}


