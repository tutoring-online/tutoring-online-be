using DataAccess.Entities.Syllabus;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;

namespace DataAccess.Repository.MySqlRepository;

public class SyllabusDao: ISyllabusDao
{
    public IEnumerable<Syllabus?> GetSyllabuses()
    {
        var syllabuses = new List<Syllabus?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate";
            var fromStatement = "From Syllabus";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                syllabuses.Add(new Syllabus
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate")),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price")
                });
            }
        }
        catch (MySqlException e)
        {
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return syllabuses;
    }

    public IEnumerable<Syllabus?> GetSyllabusById(string id)
    {
        var syllabus = new List<Syllabus?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate";
            var fromStatement = "From Syllabus";
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
                syllabus.Add(new Syllabus
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate")),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price")
                });

            }
        }
        catch (MySqlException e)
        {
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return syllabus;
    }

    public void CreateSyllabuses(IEnumerable<Syllabus> syllabuses)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@SubjectId";
            var param2 = "@Name";
            var param3 = "@Description";
            var param4 = "@TotalLessons";
            var param5 = "@Status";
            var param6 = "@Price";

            var query = "";
            var insertStatement = "Insert into Subject(SubjectId, Name, Description, TotalLessons, Status, Price) values ";

            query = insertStatement;
            for (int i = 0; i < syllabuses.Count(); i++)
            {
                if (i == syllabuses.Count() - 1)
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param5 + i}, {param6 + i})";
                else
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param5 + i}, {param6 + i})" + ",";
            }

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            for (int i = 0; i < syllabuses.Count(); i++)
            {
                Syllabus syllabus = syllabuses.ElementAt(i);
                command.Parameters.Add(param1 + i, MySqlDbType.VarChar).Value = syllabus.SubjectId;
                command.Parameters.Add(param2 + i, MySqlDbType.VarChar).Value = syllabus.Name;
                command.Parameters.Add(param3 + i, MySqlDbType.VarChar).Value = syllabus.Description;
                command.Parameters.Add(param4 + i, MySqlDbType.VarChar).Value = syllabus.TotalLessons;
                command.Parameters.Add(param5 + i, MySqlDbType.Int16).Value = syllabus.Status;
                command.Parameters.Add(param6 + i, MySqlDbType.Double).Value = syllabus.Price;
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
            LogTo.Error(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Error(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

    }
}
