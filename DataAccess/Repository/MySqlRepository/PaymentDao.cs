using Anotar.NLog;
using DataAccess.Entities.Payment;
using DataAccess.Models;
using DataAccess.Models.Payment;
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

            var selectStatement = "Select Id, SyllabusId, StudentId,CreatedDate, UpdatedDate, Status, TutorId, Combo, DateSession, StartDate, EndDate";
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
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    Combo = DbUtils.SafeGetInt16(reader, "Combo"),
                    DateSession = DbUtils.SafeGetInt16(reader, "DateSession"),
                    StartDate = DbUtils.SafeGetDateTime(reader, "StartDate"),
                    EndDate = DbUtils.SafeGetDateTime(reader, "EndDate")
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

            var selectStatement = "Select Id, SyllabusId, StudentId,CreatedDate, UpdatedDate, Status, TutorId, Combo, DateSession, StartDate, EndDate";
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
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    Combo = DbUtils.SafeGetInt16(reader, "Combo"),
                    DateSession = DbUtils.SafeGetInt16(reader, "DateSession"),
                    StartDate = DbUtils.SafeGetDateTime(reader, "StartDate"),
                    EndDate = DbUtils.SafeGetDateTime(reader, "EndDate")
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

    public void UpdatePayment(Payment payment, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(payment, connection, $"id = {id}");
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

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Payment).Name, connection, id, (int) PaymentStatus.Canceled);
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


    public Page<Payment?> GetPayments(int? limit, int? offSet, List<Tuple<string, string>> orderByParams,
        SearchPaymentDto searchPaymentDto, bool isNotPaging)
    {
        var page = new Page<Payment>();
        page.Pagination = new PageDetail();
        var payments = new List<Payment?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, StudentId, CreatedDate, UpdatedDate, Status, TutorId, Combo, DateSession, StartDate, EndDate";
            var selectCountStatement = "Select count(id) as TotalElement";
            var fromStatement = "From Payment";
            var whereStatement = $"Where (@SyllabusId is null or SyllabusId = @SyllabusId)" +
                                 $"and (@StudentId is null or StudentId = @StudentId)" +
                                 $"and (@Id is null or Id = @Id)" +
                                 $"and (@DateSession is null or DateSession = @DateSession)" +
                                 $"and (@TutorId is null or TutorId = @TutorId)" +
                                 $"and (@Combo is null or Combo = @Combo)" +
                                 $"and (@Id is null or Id = @Id)" +
                                 $"and (@FromCreatedDate is null or CreatedDate >= @FromCreatedDate)" +
                                 $"and (@ToCreatedDate is null or CreatedDate <= @ToCreatedDate)" +
                                 $"and (@FromUpdatedDate is null or UpdatedDate >= @FromUpdatedDate)" +
                                 $"and (@ToUpdatedDate is null or UpdatedDate <= @ToUpdatedDate)" +
                                 $"and (@Status is null or Status = @Status) ";
            var orderByStatement = MySqlUtils.CreateOrderByStatement(orderByParams);
            var limitStatement = $"Limit {limit} offSet {offSet}";

            if (!orderByParams.Any())
                orderByStatement = "";

            if (isNotPaging)
                limitStatement = "";

            var listStatement1 = new List<string>();
            listStatement1.Add(selectStatement);
            listStatement1.Add(fromStatement);
            listStatement1.Add(whereStatement);
            listStatement1.Add(orderByStatement);
            listStatement1.Add(limitStatement);

            var query = string.Join(" ", listStatement1);

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            command.Parameters.Add("@StudentId", MySqlDbType.VarChar).Value = searchPaymentDto.StudentId;
            command.Parameters.Add("@Id", MySqlDbType.VarChar).Value = searchPaymentDto.Id;
            command.Parameters.Add("@TutorId", MySqlDbType.VarChar).Value = searchPaymentDto.TutorId;
            command.Parameters.Add("@DateSession", MySqlDbType.Int16).Value = searchPaymentDto.DateSession;
            command.Parameters.Add("@Combo", MySqlDbType.Int16).Value = searchPaymentDto.Combo;
            command.Parameters.Add("@SyllabusId", MySqlDbType.VarChar).Value = searchPaymentDto.SyllabusId;
            command.Parameters.Add("@FromCreatedDate", MySqlDbType.DateTime).Value = searchPaymentDto.FromCreatedDate;
            command.Parameters.Add("@ToCreatedDate", MySqlDbType.DateTime).Value = searchPaymentDto.ToCreatedDate;
            command.Parameters.Add("@FromUpdatedDate", MySqlDbType.DateTime).Value = searchPaymentDto.FromUpdatedDate;
            command.Parameters.Add("@ToUpdatedDate", MySqlDbType.DateTime).Value = searchPaymentDto.ToUpdatedDate;
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = searchPaymentDto.Status;

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
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    Combo = DbUtils.SafeGetInt16(reader, "Combo"),
                    DateSession = DbUtils.SafeGetInt16(reader, "DateSession"),
                    StartDate = DbUtils.SafeGetDateTime(reader, "StartDate"),
                    EndDate = DbUtils.SafeGetDateTime(reader, "EndDate")
                });
            }
            reader.Close();
            page.Data = payments;

            var listStatement2 = new List<string>();
            listStatement2.Add(selectCountStatement);
            listStatement2.Add(fromStatement);
            listStatement2.Add(whereStatement);

            query = string.Join(" ", listStatement2);
            command.CommandText = query;
            reader = command.ExecuteReader();
            page.Pagination.TotalItems = reader.Read() ? DbUtils.SafeGetInt16(reader, "TotalElement") : 0;

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
