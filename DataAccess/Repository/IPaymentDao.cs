using DataAccess.Entities.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IPaymentDao
{
    IEnumerable<Payment?> GetPayments();

    IEnumerable<Payment?> GetPaymentById(string id);

    void CreatePayments(IEnumerable<Payment> payment);
    void UpdatePayment(Payment asEntity, string id);
}
