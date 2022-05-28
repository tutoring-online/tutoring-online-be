using DataAccess.Entities.Subject;
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

            using var command = DbUtils.CreateMySqlCommand("Select * From Subject", logger, connection);

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
}