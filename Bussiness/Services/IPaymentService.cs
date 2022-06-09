using DataAccess.Entities.Payment;
using DataAccess.Models.Payment;

namespace tutoring_online_be.Services;

public interface IPaymentService
{
    IEnumerable<PaymentDto> GetPayments();

    IEnumerable<PaymentDto> GetPaymentById(string id);

    void CreatePayments(IEnumerable<Payment> payments);
    void UpdatePayment(Payment asEntity, string id);
    
}
