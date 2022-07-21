using System.Text;
using Anotar.NLog;
using CorePush.Google;
using DataAccess.Entities.Lesson;
using DataAccess.Models;
using DataAccess.Models.Lesson;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Models.Tutor;
using DataAccess.Utils;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NLog.Fluent;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/lessons")]
public class LessonController : Controller
{
    private readonly ILessonService lessonService;
    private readonly IStudentService studentService;
    private readonly ISyllabusService syllabusService;
    private readonly ITutorService tutorService;
    private readonly HttpClient client;
    private readonly FcmSettings settings;
    private readonly IDistributedCache cache;

    public LessonController(
        ILessonService lessonService,
        IStudentService studentService,
        ITutorService tutorService,
        ISyllabusService syllabusService,
        HttpClient client,
        FcmSettings settings,
        IDistributedCache cache
        )
    {
        this.lessonService = lessonService;
        this.studentService = studentService;
        this.tutorService = tutorService;
        this.syllabusService = syllabusService;
        this.client = client;
        this.settings = settings;
        this.cache = cache;
    }

    [HttpGet]
    public IActionResult GetLessons([FromQuery]PageRequestModel model, [FromQuery]SearchLessonRequest request)
    {
        if (AppUtils.HaveQueryString(model) || AppUtils.HaveQueryString(request))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Lesson));

            Page<SearchLessonReponse> responseData = lessonService.GetLessons(model, orderByParams, request);

            return Ok(responseData);
            
        }
        
        var cacheLessons = cache.GetString("lessons");

        if (string.IsNullOrEmpty(cacheLessons))
        {
            IEnumerable<LessonDto> lessonDtos = lessonService.GetLessons();
            var serializer = JsonConvert.SerializeObject(lessonDtos);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
            cache.SetString("lessons", serializer, options);
            return Ok(JsonConvert.DeserializeObject<IEnumerable<LessonDto>>(cache.GetString("lessons")));
        }
        
        return Ok(JsonConvert.DeserializeObject<IEnumerable<LessonDto>>(cacheLessons));
                
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<LessonDto> GetLesson(string id)
    {
         // var fcm = new FcmSender(settings, client);
         // var request = fcm.SendAsync("flDS9CvtC1s13X9EExDJ6r:APA91bENFCG3ML9HCPpQTTRv-J8IqGiXXjm36GVkglqI16DEnTCALbbG7g2LQKx2_4JoP6JBGOqMglDJvZbDh1k2b4IJ2ldoGDHa0pIO39iDJOxxjhRtgtXrNs7QqbE0Ej2dImdkSIOg" ,new GoogleNotification
         // {
         //     Data = new GoogleNotification.DataPayload
         //     { 
         //         Message = "Hello world"
         //     }
         // });
         // LogTo.Info("test" + request.Result.IsSuccess());
        var lessons = lessonService.GetLessonById(id);

        return lessons;
    }
    [HttpPost]
    public IActionResult CreateLessons([FromBody]IEnumerable<CreateLessonDto> lessonDto)
    {
        IEnumerable<Lesson> lessons = lessonDto.Select(lessonDto => lessonDto.AsEntity());

        lessonService.CreateLessons(lessons);
        return Created("", "");
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateLesson(string id, [FromBody]UpdateLessonDto lessonDto)
    {
        var lessons = lessonService.GetLessonById(id);
        if (lessons.Any())
        {
            lessonService.UpdateLessons(lessonDto.AsEntity(), id);
        }

    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeleteLesson(string id)
    {
        lessonService.DeleteLesson(id);
    }
}

