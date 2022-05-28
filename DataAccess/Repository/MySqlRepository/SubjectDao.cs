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
                    Id = reader.GetString("Id"),
                    SubjectCode = reader.GetString("SubjectCode"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    CreatedDate = reader.GetMySqlDateTime("CreatedDate").ToString(),
                    UpdatedDate = reader.GetMySqlDateTime("UpdatedDate").ToString(),
                    CategoryId = reader.GetString("CategoryId")
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
                    Id = reader.GetString("Id"),
                    SubjectCode = reader.GetString("SubjectCode"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    CreatedDate = reader.GetMySqlDateTime("CreatedDate").ToString(),
                    UpdatedDate = reader.GetMySqlDateTime("UpdatedDate").ToString(),
                    CategoryId = reader.GetString("CategoryId")
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
}