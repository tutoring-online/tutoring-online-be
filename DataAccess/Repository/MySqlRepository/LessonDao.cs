using Anotar.NLog;
using DataAccess.Entities.Lesson;
using DataAccess.Models;
using DataAccess.Models.Lesson;
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

    public Page<Lesson> GetLessons(int? limit, int? offSet, List<Tuple<string, string>> orderByParams, SearchLessonRequest request, bool isNotPaging)
    {
        var page = new Page<Lesson>();
        page.Pagination = new PageDetail();
        var lessons = new List<Lesson?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SyllabusId, StudentId, TutorId, Date, SlotNumber,CreatedDate, UpdatedDate, Status";
            var selectCountStatement = "Select count(id) as TotalElement";
            var fromStatement = "From Lesson";
            var whereStatement = $"Where (@SyllabusId is null or SyllabusId = @SyllabusId)" +
                                 $"and (@StudentId is null or StudentId = @StudentId)" +
                                 $"and (@TutorId is null or TutorId = @TutorId)" +
                                 $"and (@FromCreatedDate is null or CreatedDate >= @FromCreatedDate)" +
                                 $"and (@ToCreatedDate is null or CreatedDate <= @ToCreatedDate)" +
                                 $"and (@FromUpdatedDate is null or UpdatedDate >= @FromUpdatedDate)" +
                                 $"and (@ToUpdatedDate is null or UpdatedDate <= @ToUpdatedDate)" +
                                 $"and (@FromDate is null or Date >= @FromDate)" +
                                 $"and (@ToDate is null or Date <= @ToDate) " +
                                 $"and (@Status is null or Status = @Status)" +
                                 $"and (@SlotNumber is null or SlotNumber in (@SlotNumber))";
            var orderByStatement = MySqlUtils.CreateOrderByStatement(orderByParams);
            var limitStatement = $"Limit {limit} offSet {offSet}";

            if (!orderByParams.Any())
                orderByStatement = "";

            if (isNotPaging)
                limitStatement = "";

            var listStatement1 = new List<string>();
            listStatement1.Add(selectStatement);
            listStatement1.Add(fromStatement);
            listStatement1.Add(whereStatement);
            listStatement1.Add(orderByStatement);
            listStatement1.Add(limitStatement);

            var query = string.Join(" ", listStatement1);

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            command.Parameters.Add("@StudentId", MySqlDbType.VarChar).Value = request.StudentId;
            command.Parameters.Add("@TutorId", MySqlDbType.VarChar).Value = request.TutorId;
            command.Parameters.Add("@SyllabusId", MySqlDbType.VarChar).Value = request.SyllabusId;
            command.Parameters.Add("@FromCreatedDate", MySqlDbType.DateTime).Value = request.FromCreatedDate;
            command.Parameters.Add("@ToCreatedDate", MySqlDbType.DateTime).Value = request.ToCreatedDate;
            command.Parameters.Add("@FromUpdatedDate", MySqlDbType.DateTime).Value = request.FromUpdatedDate;
            command.Parameters.Add("@ToUpdatedDate", MySqlDbType.DateTime).Value = request.ToUpdatedDate;
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = request.Status;
            command.Parameters.Add("@FromDate", MySqlDbType.DateTime).Value = request.FromDate;
            command.Parameters.Add("@ToDate", MySqlDbType.DateTime).Value = request.ToDate;
            command.Parameters.Add("@SlotNumber", MySqlDbType.VarString).Value =
                request.SlotNumber is null || !request.SlotNumber.Any() 
                    ? null 
                    : string.Join(",", request.SlotNumber);

            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                lessons.Add(new Lesson()
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SyllabusId = DbUtils.SafeGetString(reader, "SyllabusId"),
                    StudentId = DbUtils.SafeGetString(reader, "StudentId"),
                    TutorId = DbUtils.SafeGetString(reader, "TutorId"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Date = DbUtils.SafeGetDateTime(reader, "Date")
                });
            }
            reader.Close();
            page.Data = lessons;

            var listStatement2 = new List<string>();
            listStatement2.Add(selectCountStatement);
            listStatement2.Add(fromStatement);
            listStatement2.Add(whereStatement);

            query = string.Join(" ", listStatement2);
            command.CommandText = query;
            reader = command.ExecuteReader();
            page.Pagination.TotalItems = reader.Read() ? DbUtils.SafeGetInt16(reader, "TotalElement") : 0;

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

        return page;
    }
}
