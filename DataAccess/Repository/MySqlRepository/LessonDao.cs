using DataAccess.Entities.Lesson;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class LessonDao:ILessonDao
{
    private readonly ILogger<LessonDao> logger;

    public LessonDao(ILogger<LessonDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Lesson?> GetLessons()
    {
        var lessons = new List<Lesson?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
            connection.Open();

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumer, Date, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Lesson";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, logger, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lessons.Add(new Lesson
                {
                    Id = reader.GetString("Id"),
                    SyllabusId = reader.GetString("SyllabusId"),
                    TutorId = reader.GetString("TutorId"),
                    StudentId = reader.GetString("StudentId"),
                    Status = reader.GetInt16("Status"),
                    SlotNumer = reader.GetInt16("SlotNumer"),
                    Date = reader.GetMySqlDateTime("Date").ToString(),
                    CreatedDate = reader.GetMySqlDateTime("CreatedDate").ToString(),
                    UpdatedDate = reader.GetMySqlDateTime("UpdatedDate").ToString()
                    
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

        return lessons;
    }
    public IEnumerable<Lesson?> GetLessonById(string id)
    {
        var lessons = new List<Lesson?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, TutorId, StudentId, SlotNumer, Date, CreatedDate, UpdatedDate, Status";
            var fromStatement = "From Lesson";
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
                lessons.Add(new Lesson
                {
                    Id = reader.GetString("Id"),
                    SyllabusId = reader.GetString("SyllabusId"),
                    TutorId = reader.GetString("TutorId"),
                    StudentId = reader.GetString("StudentId"),
                    Status = reader.GetInt16("Status"),
                    SlotNumer = reader.GetInt16("SlotNumer"),
                    Date = reader.GetMySqlDateTime("Date").ToString(),
                    CreatedDate = reader.GetMySqlDateTime("CreatedDate").ToString(),
                    UpdatedDate = reader.GetMySqlDateTime("UpdatedDate").ToString()
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

        return lessons;
    }

    public void CreateLessons(IEnumerable<Lesson> lessons)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection(logger);
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
                    query = query + " " + $"({param1 + i},{param2 + i},{param3 + i},{param4 + i}, {param4 + i})" + ",";
            }

            using var command = DbUtils.CreateMySqlCommand(query, logger, connection);
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



}
