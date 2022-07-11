using Anotar.NLog;
using DataAccess.Entities.TutorSubject;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.MySqlRepository;

public class TutorSubjectDao:ITutorSubjectDao
{
    private readonly ILogger<TutorSubjectDao> logger;

    public TutorSubjectDao(ILogger<TutorSubjectDao> logger)
    {
        this.logger = logger;
    }
    public IEnumerable<TutorSubject?> GetTutorSubjects()
    {
        var tutorSubject = new List<TutorSubject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, TutorId, SubjectId, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From TutorSubject";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tutorSubject.Add(new TutorSubject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),                  
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Status = DbUtils.SafeGetInt16(reader, "Status")
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

        return tutorSubject;
    }
    public Dictionary<string, TutorSubject?> GetTutorSubjects(HashSet<string> ids)
    {
        var tutorSubjects = new Dictionary<string, TutorSubject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, TutorId, SubjectId, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From TutorSubject";
            var whereSatement = $"Where id In ({MySqlUtils.CreateInStatementValues(ids)})";
            var query = MySqlUtils.ConstructQueryByStatements(new List<string>
            {
                selectStatement,
                fromStatement,
                whereSatement
            });

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tutorSubjects.Add(
                    DbUtils.SafeGetString(reader, "Id"),
                    new TutorSubject()
                    {
                        Id = DbUtils.SafeGetString(reader, "Id"),
                        TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                        SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                        CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                        UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                        Status = DbUtils.SafeGetInt16(reader, "Status")
                    }
                    );

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

        return tutorSubjects;
    }
    public IEnumerable<TutorSubject?> GetTutorSubjectById(string id)
    {
        var tutorSubjects = new List<TutorSubject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, TutorId, SubjectId, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From TutorSubject";
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
                tutorSubjects.Add(new TutorSubject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Status = DbUtils.SafeGetInt16(reader, "Status")
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

        return tutorSubjects;
    }
    public int CreateTutorSubject(TutorSubject tutorSubject)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            IEnumerable<TutorSubject> tutorSubjects = new[]
            {
                tutorSubject
            };

            using var command = MySqlUtils.CreateInsertStatement(tutorSubjects, connection);
            return command.ExecuteNonQuery();
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

        return 0;
    }
    public void UpdateTutorSubject(TutorSubject asEntity, string id)
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
    }
    public int DeleteTutorSubject(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(TutorSubject).Name, connection, id, (int)TutorSubjectStatus.Deleted);
            return command.ExecuteNonQuery();
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

        return 0;
    }

    public void CreateTutorSubject(IEnumerable<TutorSubject> tutorSubjects)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateInsertStatement(tutorSubjects, connection);
            command.ExecuteNonQuery();
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
    }

    public IEnumerable<TutorSubject> GetTutorSubjectsByTutorId(string? tutorDtoId)
    {
        var tutorSubjects = new List<TutorSubject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, TutorId, SubjectId, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From TutorSubject";
            var whereStatement = $"Where TutorId = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = tutorDtoId;
            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tutorSubjects.Add(new TutorSubject
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Status = DbUtils.SafeGetInt16(reader, "Status")
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

        return tutorSubjects;
    }
}
