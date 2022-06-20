using DataAccess.Entities.Payment;
using DataAccess.Models;
using DataAccess.Models.Payment;

namespace tutoring_online_be.Services;

public interface IPaymentService
{
    IEnumerable<PaymentDto> GetPayments();
    
    IEnumerable<PaymentDto> GetPaymentById(string id);

    void CreatePayments(IEnumerable<Payment> payments);
    void UpdatePayment(Payment asEntity, string id);

    int DeletePayment(string id);
    
    public delegate IPaymentService ServiceResolver(string key);

    Page<PaymentDto> GetPayments(PageRequestModel model, List<Tuple<string, string>> orderByParams);
}
