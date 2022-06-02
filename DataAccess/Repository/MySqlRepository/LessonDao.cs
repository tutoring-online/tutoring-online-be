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

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumer, Date, CreatedDate, UpdatedDate, Status";
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
                    SlotNumer = DbUtils.SafeGetInt16(reader, "SlotNumer"),
                    Date = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "Date")),
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

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumer, Date, CreatedDate, UpdatedDate, Status";
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
                    SlotNumer = DbUtils.SafeGetInt16(reader, "SlotNumer"),
                    Date = CommonUtils.ConvertDateTimeToString(DbUtils.SafeGetDateTime(reader, "Date")),
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

        return lessons;
    }

    public void CreateLessons(IEnumerable<Lesson> lessons)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@SyllabusId";
            var param2 = "@TutorId";
            var param3 = "@StudentId";
            var param4 = "@SlotNumer";
            var param5 = "@Status";

            var query = "";
            var insertStatement = "Insert into Lesson(SyllabusId, TutorId, StudentId, SlotNumer, Status) values ";

            query = insertStatement;
            for (int i = 0; i < lessons.Count(); i++)
            {
                if (i == lessons.Count() - 1)
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param5 + i})";
                else
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param5 + i})" + ",";
            }

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            for (int i = 0; i < lessons.Count(); i++)
            {
                Lesson lesson = lessons.ElementAt(i);
                command.Parameters.Add(param1 + i, MySqlDbType.VarChar).Value = lesson.SyllabusId;
                command.Parameters.Add(param2 + i, MySqlDbType.VarChar).Value = lesson.TutorId;
                command.Parameters.Add(param3 + i, MySqlDbType.VarChar).Value = lesson.StudentId;
                command.Parameters.Add(param4 + i, MySqlDbType.VarChar).Value = lesson.SlotNumer;
                command.Parameters.Add(param5 + i, MySqlDbType.Int16).Value = lesson.Status;
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



}
