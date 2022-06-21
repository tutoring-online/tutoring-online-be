using Anotar.NLog;
using DataAccess.Entities.Payment;
using DataAccess.Entities.Student;
using DataAccess.Models;
using DataAccess.Models.Payment;
using DataAccess.Models.Student;
using DataAccess.Models.Syllabus;
using DataAccess.Utils;
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

    public PaymentController(
        ServiceResolver serviceResolver,
        IStudentService studentService,
        ISyllabusService syllabusService
        )
    {
        this.paymentService = serviceResolver("payment-v1");
        this.studentService = studentService;
        this.syllabusService = syllabusService;
    }
    
    [HttpGet]
    public IActionResult GetPayments([FromQuery]PageRequestModel model)
    {
        if (AppUtils.HaveQueryString(model))
        {
            var orderByParams = AppUtils.SortFieldParsing(model.Sort, typeof(Payment));
            
            Page<PaymentDto> responseData = paymentService.GetPayments(model, orderByParams);

            if (responseData.Data is not null || responseData.Data.Count > 0)
            {
                List<PaymentDto> paymentDtos = responseData.Data;
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
    public void UpdatePayment(string id, UpdatePaymentDto updatePaymentDto)
    {
        var payments = paymentService.GetPaymentById(id);
        if (payments.Any())
        {
            paymentService.UpdatePayment(updatePaymentDto.AsEntity(), id);
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeletePayment(string id)
    {
        paymentService.DeletePayment(id);
    }
}
