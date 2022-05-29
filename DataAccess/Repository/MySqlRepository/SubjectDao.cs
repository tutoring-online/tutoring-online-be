using System.Data;
using DataAccess.Entities.Subject;
using DataAccess.Models.Subject;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class SubjectDao : ISubjectDao
{
    private readonly ILogger<SubjectDao> logger;

    public SubjectDao(ILogger<SubjectDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Subject?> GetSubjects()
    {
        var subjects= new List<Subject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
            connection.Open();

            var selectStatement = "Select Id, SubjectCode, Name, Description, Status, CreatedDate, UpdatedDate, CategoryId";
            var fromStatement = "From Subject";
            var query = selectStatement + " " + fromStatement;
                                  
            using var command = DbUtils.CreateMySqlCommand(query, logger, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                subjects.Add(new Subject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectCode = DbUtils.SafeGetString(reader, "SubjectCode"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate")),
                    CategoryId = DbUtils.SafeGetString(reader, "CategoryId")
                });
            }
        }
        catch (MySqlException e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        catch (Exception e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection(logger);
        }

        return subjects;
    }

    public IEnumerable<Subject?> GetSubjectById(string id)
    {
        var subjects= new List<Subject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SubjectCode, Name, Description, Status, CreatedDate, UpdatedDate, CategoryId";
            var fromStatement = "From Subject";
            var whereStatement = $"Where Id = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement; 
                                  
            using var command = DbUtils.CreateMySqlCommand(query, logger, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = id;
            command.Prepare();
            
            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                logger.LogInformation(LogUtils.CreateLogMessage($"Param {commandParameter}: {commandParameter.Value}"));
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                subjects.Add(new Subject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectCode = DbUtils.SafeGetString(reader, "SubjectCode"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "CreatedDate")),
                    UpdatedDate = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "UpdatedDate")),
                    CategoryId = DbUtils.SafeGetString(reader, "CategoryId")
                });

            }
        }
        catch (MySqlException e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        catch (Exception e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection(logger);
        }

        return subjects;
    }

    public void CreateSubjects(IEnumerable<Subject> subjects)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
            connection.Open();
            var param1 = "@SubjectCode";
            var param2 = "@Name";
            var param3 = "@Description";
            var param4 = "@CategoryId";
            var param5 = "@Status";
            
            var query = "";
            var insertStatement = "Insert into Subject(SubjectCode, Name, Description, CategoryId, Status) values ";

            query = insertStatement;
            for (int i = 0; i < subjects.Count(); i++)
            {
                if (i == subjects.Count() - 1)
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param5 + i})";
                else
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param4 + i})" + ",";
            }

            using var command = DbUtils.CreateMySqlCommand(query, logger, connection);
            command.CommandText = query;

            for (int i = 0; i < subjects.Count(); i++)
            {
                Subject subject = subjects.ElementAt(i);
                command.Parameters.Add(param1 + i, MySqlDbType.VarChar).Value = subject.SubjectCode;
                command.Parameters.Add(param2 + i, MySqlDbType.VarChar).Value = subject.Name;
                command.Parameters.Add(param3 + i, MySqlDbType.VarChar).Value = subject.Description;
                command.Parameters.Add(param4 + i, MySqlDbType.VarChar).Value = subject.CategoryId;
                command.Parameters.Add(param5 + i, MySqlDbType.Int16).Value = subject.Status;
            }
            
            command.Prepare();
            
            Console.WriteLine(command.CommandText);
            
            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                logger.LogInformation(LogUtils.CreateLogMessage($"Param {commandParameter}: {commandParameter.Value}"));
            }

            command.ExecuteNonQuery();

        }
        catch (MySqlException e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        catch (Exception e)
        {
            logger.LogInformation(LogUtils.CreateLogMessage(e.ToString()));
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection(logger);
        }

    }

    public void UpdateSubjects(Subject subject)
    {
        
    }
}