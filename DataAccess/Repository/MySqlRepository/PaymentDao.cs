﻿using Anotar.NLog;
using DataAccess.Entities.Payment;
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
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate"))
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
            var fromStatement = "From Lesson";
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
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate"))
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
            var param1 = "@SyllabusId";
            var param2 = "@StudentId";
            var param3 = "@Status";

            var query = "";
            var insertStatement = "Insert into Lesson(SyllabusId, StudentId, Status) values ";

            query = insertStatement;
            for (int i = 0; i < payments.Count(); i++)
            {
                if (i == payments.Count() - 1)
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i})";
                else
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i})" + ",";
            }

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            for (int i = 0; i < payments.Count(); i++)
            {
                Payment payment = payments.ElementAt(i);
                command.Parameters.Add(param1 + i, MySqlDbType.VarChar).Value = payment.SyllabusId;
                command.Parameters.Add(param2 + i, MySqlDbType.VarChar).Value = payment.StudentId;
                command.Parameters.Add(param3 + i, MySqlDbType.Int16).Value = payment.Status;
               
            }

            command.Prepare();

            Console.WriteLine(command.CommandText);

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

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

}
