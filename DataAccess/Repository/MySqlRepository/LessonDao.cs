using Anotar.NLog;
using DataAccess.Entities.Lesson;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class LessonDao:ILessonDao
{
    public LessonDao()
    {
    }

    public IEnumerable<Lesson?> GetLessons()
    {
        var lessons = new List<Lesson?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumber, Date, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Lesson";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lessons.Add(new Lesson
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    SlotNumber = DbUtils.SafeGetInt16(reader, "SlotNumber"),
                    Date = DbUtils.SafeGetDateTime(reader, "Date"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
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

        return lessons;
    }
    public IEnumerable<Lesson?> GetLessonById(string id)
    {
        var lessons = new List<Lesson?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumber, Date, CreatedDate, UpdatedDate, Status";
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
                lessons.Add(new Lesson
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    SlotNumber = DbUtils.SafeGetInt16(reader, "SlotNumber"),
                    Date = DbUtils.SafeGetDateTime(reader, "Date"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
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

        return lessons;
    }

    public void CreateLessons(IEnumerable<Lesson> lessons)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateInsertStatement(lessons, connection);
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

    public void UpdateLessons(Lesson lesson, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(lesson, connection, $"id = {id}");
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

    public int DeleteLesson(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Lesson).Name, connection, id);
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
}
