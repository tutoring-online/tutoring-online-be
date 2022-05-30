﻿using DataAccess.Entities.Payment;
using DataAccess.Models.Payment;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route ("/api/v1/payments")]
public class PaymentController
{
    private readonly IPaymentService paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        this.paymentService = paymentService;
    }

    [HttpGet]
    public IEnumerable<PaymentDto> GetPayments()
    {
        return paymentService.GetPayments();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<PaymentDto> GetPayment(string id)
    {
        var payments = paymentService.GetPaymentById(id);

        return payments;
    }
    [HttpPost]
    public void CreatePayments(IEnumerable<PaymentDto> paymentDto)
    {
        IEnumerable<Payment> payments = paymentDto.Select(paymentDto => paymentDto.AsEntity());

        paymentService.CreatePayments(payments);

    }
}