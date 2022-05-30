﻿using DataAccess.Entities.Payment;
using DataAccess.Models.Payment;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class PaymentServiceV1 : IPaymentService
{
    private readonly IPaymentDao paymentDao;

    public PaymentServiceV1(IPaymentDao paymentDao)
    {
        this.paymentDao = paymentDao;
    }

    public IEnumerable<PaymentDto> GetPayments()
    {
        return paymentDao.GetPayments().Select(payment => payment.AsDtoPayments());
    }

    public IEnumerable<PaymentDto> GetPaymentById(string id)
    {
        return paymentDao.GetPaymentById(id).Select(payment => payment.AsDtoPayments());
    }

    public void CreatePayments(IEnumerable<Payment> payments)
    {
        paymentDao.CreatePayments(payments);
    }
}