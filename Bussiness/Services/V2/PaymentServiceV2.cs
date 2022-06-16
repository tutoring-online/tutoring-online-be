using DataAccess.Entities.Payment;
using DataAccess.Models.Payment;

namespace tutoring_online_be.Services.V2;

public class PaymentServiceV2 : IPaymentService
{
    public IEnumerable<PaymentDto> GetPayments()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PaymentDto> GetPaymentById(string id)
    {
        throw new NotImplementedException();
    }

    public void CreatePayments(IEnumerable<Payment> payments)
    {
        throw new NotImplementedException();
    }

    public void UpdatePayment(Payment asEntity, string id)
    {
        throw new NotImplementedException();
    }

    public int DeletePayment(string id)
    {
        throw new NotImplementedException();
    }
}