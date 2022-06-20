using Anotar.NLog;
using DataAccess.Entities.Payment;
using DataAccess.Models;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class PaymentDao : IPaymentDao
{
    public IEnumerable<Payment?> GetPayments()
    {
        var payments = new List<Payment?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SyllabusId, StudentId,CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Payment";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                payments.Add(new Payment
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),            
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),              
                    CreatedDate =DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate")
                });

            }
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return payments;
    }
    public IEnumerable<Payment?> GetPaymentById(string id)
    {
        var payments = new List<Payment?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, StudentId,CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Payment";
            var whereStatement = $"Where Id = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = id;
            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                payments.Add(new Payment
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate")
                });

            }
        }
        catch (MySqlException e)
        { 
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return payments;
    }

    public void CreatePayments(IEnumerable<Payment> payments)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateInsertStatement(payments, connection);
            
            command.ExecuteNonQuery();

        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

    }

    public void UpdatePayment(Payment asEntity, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(asEntity, connection, $"id = {id}");
            command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        };
    }

    public int DeletePayment(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Payment).Name, connection, id);
            return command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        };

        return 0;
    }


    public Page<Payment?> GetPayments(int? limit, int? offSet, List<Tuple<string, string>> orderByParams,bool isNotPaging)
    {
        var page = new Page<Payment>();
        page.Pagination = new PageDetail();
        var payments = new List<Payment?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, StudentId,CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Payment";
            var whereStatement = $"";
            var orderByStatement = $"Order by ";
            var limitStatement = $"Limit {limit} offSet {offSet}";

            for (var index = 0; index < orderByParams.Count; index++)
            {
                var orderByParam = orderByParams[index];
                if (orderByParam.Item2.Equals("+"))
                {
                    if (index == 0)
                    {
                        orderByStatement += $"{orderByParam.Item1} ASC ";
                    }
                    else
                    {
                        orderByStatement += $",{orderByParam.Item1} ASC ";
                    }
                }

                else if (orderByParam.Item2.Equals("-"))
                {
                    if (index == 0)
                    {
                        orderByStatement += $"{orderByParam.Item1} DESC ";
                    }
                    else
                    {
                        orderByStatement += $",{orderByParam.Item1} DESC ";
                    }
                }
                    
            }

            if (!orderByParams.Any())
                orderByStatement = "";

            if (isNotPaging is true)
                limitStatement = "";
                
            var query = selectStatement + " " + fromStatement + " " + whereStatement + " " + orderByStatement + " " + limitStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                payments.Add(new Payment
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate")
                });
            }
            reader.Close();
            
            page.Data = payments;

            var selectCountStatement = "Select count(id) as TotalElement";
            query = selectCountStatement + " " + fromStatement + " " + whereStatement + " " + orderByStatement + " " + limitStatement;

            command.CommandText = query;

            reader = command.ExecuteReader();

            if (reader.Read())
            {
                page.Pagination.TotalItems = DbUtils.SafeGetInt16(reader, "TotalElement");
            }
            else
            {
                page.Pagination.TotalItems = 0;
            }

        }
        catch (MySqlException e)
        { 
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return page;
    }
}
