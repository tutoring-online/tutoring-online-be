using Anotar.NLog;
using DataAccess.Entities.Payment;
using DataAccess.Models;
using DataAccess.Models.Payment;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Fluent;
using tutoring_online_be.Controllers.Utils;
using tutoring_online_be.Security;
using tutoring_online_be.Services;
using static tutoring_online_be.Services.IPaymentService;

namespace tutoring_online_be.Controllers.V1;

[ValidateModel]
[Route ("/api/v1/payments")]
public class PaymentController : Controller
{
    private readonly IPaymentService paymentService;

    public PaymentController(ServiceResolver serviceResolver)
    {
        this.paymentService = serviceResolver("payment-v1");
    }
    
    [HttpGet]
    public IActionResult GetPayments([FromQuery]PageRequestModel model)
    {
        if (CommonUtils.HaveQueryString(model))
        {
            return Ok();
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
