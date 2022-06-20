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

        int? limit = 0;
        int? offSet = 0;
        var isNotPaging = false;
        var page = model.Page is null ? 0 : model.Page;
        var size = model.Size switch
        {
            null => 10,
            > 0 => model.Size,
            _ => 0
        };

        if (size == 0)
            isNotPaging = true;

        limit = size;
        offSet = page * size;

        result = paymentDao.GetPayments(limit, offSet, orderByParams, isNotPaging);

        resultDto.Data = result.Data.Select(p => p.AsDto()).ToList();
        resultDto.Pagination = result.Pagination;
        resultDto.Pagination.Page = page;
        resultDto.Pagination.Size = size;
        
        int totalPage = (int)(resultDto.Pagination.TotalItems / resultDto.Pagination.Size);
        if (totalPage < 1)
            totalPage = 1;
        else if (resultDto.Pagination.TotalItems % resultDto.Pagination.Size > 0)
            totalPage += 1;
        
        resultDto.Pagination.TotalPages = totalPage;

        return resultDto;
    }
}
