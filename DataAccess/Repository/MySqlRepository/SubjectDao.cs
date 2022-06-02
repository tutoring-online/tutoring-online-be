using System.Data;
using Anotar.NLog;
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
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Code, Name, Description, Status, CreatedDate, UpdatedDate, CategoryId";
            var fromStatement = "From Subject";
            var query = selectStatement + " " + fromStatement;
                                  
            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                subjects.Add(new Subject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Code = DbUtils.SafeGetString(reader, "Code"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate =DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    CategoryId = DbUtils.SafeGetString(reader, "CategoryId")
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

        return subjects;
    }

    public IEnumerable<Subject?> GetSubjectById(string id)
    {
        var subjects= new List<Subject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Code, Name, Description, Status, CreatedDate, UpdatedDate, CategoryId";
            var fromStatement = "From Subject";
            var whereStatement = $"Where Id = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement; 
                                  
            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = id;
            command.Prepare();
            
            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Error($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                subjects.Add(new Subject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Code = DbUtils.SafeGetString(reader, "Code"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    CategoryId = DbUtils.SafeGetString(reader, "CategoryId")
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

        return subjects;
    }

    public void CreateSubjects(IEnumerable<Subject> subjects)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            
            DateTime currentTime = DateTime.Now;

            //Todo update later for CreateInsertStatement
            subjects.Select(s => s.CreatedDate = currentTime);

            using var command = MySqlUtils.CreateInsertStatement(subjects, connection);
            
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

    public void UpdateSubjects(Subject subject)
    {
        
    }
}