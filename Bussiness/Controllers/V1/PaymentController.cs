using Anotar.NLog;
using DataAccess.Entities.Lesson;
using DataAccess.Entities.Payment;
using DataAccess.Entities.Student;
using DataAccess.Models;
using DataAccess.Models.Lesson;
using DataAccess.Models.Payment;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Utils;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Fluent;
using tutoring_online_be.Controllers.Utils;
using tutoring_online_be.Security;
using tutoring_online_be.Services;
using tutoring_online_be.Utils;
using static tutoring_online_be.Services.IPaymentService;

namespace tutoring_online_be.Controllers.V1;

[ValidateModel]
[Route ("/api/v1/payments")]
public class PaymentController : Controller
{
    private readonly IPaymentService paymentService;
    private readonly IStudentService studentService;
    private readonly ISyllabusService syllabusService;
    private readonly ILessonService lessonService;

    public PaymentController(
        ServiceResolver serviceResolver,
        IStudentService studentService,
        ISyllabusService syllabusService,
        ILessonService lessonService
        )
    {
        this.paymentService = serviceResolver("payment-v1");
        this.studentService = studentService;
        this.syllabusService = syllabusService;
        this.lessonService = lessonService;
    }
    
    [HttpGet]
    public IActionResult GetPayments([FromQuery]PageRequestModel model, [FromQuery]SearchPaymentDto searchPaymentDto)
    {
        if (AppUtils.HaveQueryString(model) ||  AppUtils.HaveQueryString(searchPaymentDto))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Payment));
            
            Page<SearchPaymentDto> responseData = paymentService.GetPayments(model, orderByParams, searchPaymentDto);

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<SearchPaymentDto> paymentDtos = responseData.Data;
                HashSet<string> studentIds = paymentDtos.Select(t => t.StudentId).NotEmpty().ToHashSet();
                HashSet<string> syllabusIds = paymentDtos.Select(t => t.SyllabusId).NotEmpty().ToHashSet(); 

                if (studentIds.Count > 0)
                {
                    Dictionary<string, StudentDto> studentDtos = studentService.GetStudents(studentIds);
                    
                    foreach (var paymentDto in paymentDtos.Where(paymentDto => studentDtos.ContainsKey(paymentDto.StudentId)))
                    {
                        paymentDto.Student = studentDtos[paymentDto.StudentId];
                    }
                }

                if (syllabusIds.Count > 0)
                {
                    Dictionary<string, SyllabusDto> syllabusDtos = syllabusService.GetSyllabuses(syllabusIds);
                
                    foreach (var paymentDto in paymentDtos.Where(paymentDto => syllabusDtos.ContainsKey(paymentDto.SyllabusId)))
                    {
                        paymentDto.Syllabus = syllabusDtos[paymentDto.SyllabusId];
                    }
                }

                
            }

            return Ok(responseData);
        }

        return Ok(paymentService.GetPayments());
    }
    

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<PaymentDto> GetPayment(string id)
    {
        var payments = paymentService.GetPaymentById(id);

        return payments;
    }
    
    [HttpPost]
    public void CreatePayments(IEnumerable<CreatePaymentDto> paymentDto)
    {
        IEnumerable<Payment> payments = paymentDto.Select(paymentDto => paymentDto.AsEntity());

        paymentService.CreatePayments(payments);
    }
    
    [HttpPatch]
    [Route("{id}")]
    public IActionResult UpdatePayment(string id, [FromBody]UpdatePaymentDto updatePaymentDto)
    {
        var payments = paymentService.GetPaymentById(id);
        if (payments.Any())
        {
            LogTo.Info("\nDo update Payment");
            if (payments.ElementAt(0).Status == (int)PaymentStatus.Paid && payments.ElementAt(0).TutorId is not null)
            {
                LogTo.Info($"\nThis payment already completed! Id : {payments.ElementAt(0).Id}");
                return BadRequest();
            }

            
            paymentService.UpdatePayment(updatePaymentDto.AsEntity(), id);
            
            LogTo.Info("\nUpdate Payment Success - Do check Payment Status");
            payments = paymentService.GetPaymentById(id);
            if (payments.Any())
            {
                PaymentDto paymentDto = payments.ElementAt(0);
                int? paymentStatus = paymentDto.Status;
                string? tutorId = paymentDto.TutorId;

                if (!string.IsNullOrEmpty(tutorId) 
                    && paymentStatus is not null 
                    && paymentStatus == (int)PaymentStatus.Paid)
                {
                    LogTo.Info($"\nPayment status is {paymentStatus} - Do create lessons");
                    
                    LogTo.Info("\n Do get Syllabus");
                    SyllabusDto syllabusDto = syllabusService.GetSyllabusById(paymentDto.SyllabusId).ElementAt(0);
                    int? totalLessons = syllabusDto.TotalLessons;
                    int? numberOfWeek = totalLessons / 3;
                    int? combo = paymentDto.Combo;
                    
                    LogTo.Info($"\n Syllabus id : {syllabusDto.Id}" +
                               $"\n Total lessons: {totalLessons}" +
                               $"\n Number of studied week: {numberOfWeek}" +
                               $"\n Combo : {combo}");
                    
                    LogTo.Info("\n Do calculate start day ");
                    DayOfWeek dayOfWeek = DayOfWeek.Monday;

                    if (combo == (int)Combo.EVEN_DAYS)
                    {
                        LogTo.Info($"\nCombo : {combo} - Start day is next tuesday");
                        dayOfWeek = DayOfWeek.Tuesday;
                    }
                    else
                    {
                        LogTo.Info($"\nCombo : {combo} - Start day is next monday");
                    }
                    
                    DateTime startDay = GetNextWeekday(DateTime.Today, dayOfWeek);
                    if (DateTime.Today.DayOfWeek.Equals(dayOfWeek))
                    {
                        startDay = startDay.AddDays(7);
                    }
                    LogTo.Info($"\nStartDay is  : {startDay} - Start generate lessons for {numberOfWeek} weeks");

                    string? paymentId = paymentDto.Id;
                    int lessonsStatus = (int)LessonStatus.Pending;
                    int lessonCount = 1;
                    List<Lesson> lessonDtos = new List<Lesson>();
                    DateTime tmp1 = startDay;
                    DateTime tmp2 = startDay.AddDays(2);
                    DateTime tmp3 = startDay.AddDays(4);
                    if (combo == (int)Combo.EVEN_DAYS)
                    {
                        int count = 0;
                        do
                        {
                            if(count > 0)
                            {
                                tmp1 = tmp1.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp1,
                                SlotNumber = lessonCount++
                            });
                            
                            if (count > 0)
                            {
                                tmp2 = tmp2.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp2,
                                SlotNumber = lessonCount++
                            });
                            
                            if (count > 0)
                            {
                                tmp3 = tmp3.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp3,
                                SlotNumber = lessonCount++
                            });
                        } while (++count < numberOfWeek);
                    }
                    else
                    {
                        int count = 0;
                        do
                        {
                            if(count > 0)
                            {
                                tmp1 = tmp1.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp1,
                                SlotNumber = lessonCount++
                            });
                            
                            if (count > 0)
                            {
                                tmp2 = tmp2.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp2,
                                SlotNumber = lessonCount++
                            });
                            
                            if (count > 0)
                            {
                                tmp3 = tmp3.AddDays(7);
                            }
                            
                            lessonDtos.Add(new Lesson()
                            {
                                PaymentId = paymentId,
                                Status = lessonsStatus,
                                Date = tmp3,
                                SlotNumber = lessonCount++
                            });
                        } while (++count < numberOfWeek);
                    }
                    LogTo.Info("\n Do create lessons");
                    lessonService.CreateLessons(lessonDtos);
                    return Ok();
                }
            }

        }

        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeletePayment(string id)
    {
        paymentService.DeletePayment(id);
    }
    
    private DateTime GetNextWeekday(DateTime start, DayOfWeek day)
    {
        int daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;
        return start.AddDays(daysToAdd);
    }
}
