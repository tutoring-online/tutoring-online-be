using DataAccess.Entities.Payment;
using DataAccess.Models;
using DataAccess.Models.Payment;
using DataAccess.Repository;
using DataAccess.Utils;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

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
        return paymentDao.GetPayments().Select(payment => payment.AsDto());
    }

    public IEnumerable<PaymentDto> GetPaymentById(string id)
    {
        return paymentDao.GetPaymentById(id).Select(payment => payment.AsDto());
    }

    public void CreatePayments(IEnumerable<Payment> payments)
    {
        paymentDao.CreatePayments(payments);
    }

    public void UpdatePayment(Payment asEntity, string id)
    {
        paymentDao.UpdatePayment(asEntity, id);
    }

    public int DeletePayment(string id)
    {
        return paymentDao.DeletePayment(id);
    }

    public Page<PaymentDto> GetPayments(PageRequestModel model, List<Tuple<string, string>> orderByParams)
    {
        Page<Payment> result = new Page<Payment>();
        Page<PaymentDto> resultDto = new Page<PaymentDto>();

        result = paymentDao.GetPayments(model.GetLimit(), model.GetOffSet(), orderByParams, model.IsNotPaging());

        resultDto.Data = result.Data.Select(p => p.AsDto()).ToList();
        resultDto.Pagination = result.Pagination;
        resultDto.Pagination.Page = model.GetPage();
        resultDto.Pagination.Size = model.GetSize();
        if (model.IsNotPaging())
        {
            resultDto.Pagination.TotalPages = 1;
            resultDto.Pagination.Size = resultDto.Pagination.TotalItems;
        }
        else
        {
            resultDto.Pagination.UpdateTotalPages();
        }
        
        return resultDto;
    }
}
