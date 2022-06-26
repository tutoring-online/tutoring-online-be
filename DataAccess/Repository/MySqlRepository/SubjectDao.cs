using System.Data;
using Anotar.NLog;
using DataAccess.Entities.Subject;
using DataAccess.Models;
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

    public void UpdateSubjects(Subject subject, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(subject, connection, $"id = {id}");
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

    public int DeleteSubject(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Subject).Name, connection, id);
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

    public Page<Subject> GetSubjects(int? limit, int? offSet, List<Tuple<string, string>> orderByParams, SearchSubjectRequest request, bool isNotPaging)
    {
        var page = new Page<Subject>();
        page.Pagination = new PageDetail();
        var subjects = new List<Subject?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Code, Name, Description,CreatedDate, UpdatedDate, Status";
            var selectCountStatement = "Select count(id) as TotalElement";
            var fromStatement = "From Subject";
            var whereStatement = "Where (@FromCreatedDate is null or CreatedDate >= @FromCreatedDate)" +
                                 $"and (@ToCreatedDate is null or CreatedDate <= @ToCreatedDate)" +
                                 $"and (@FromUpdatedDate is null or UpdatedDate >= @FromUpdatedDate)" +
                                 $"and (@ToUpdatedDate is null or UpdatedDate <= @ToUpdatedDate)" +
                                 $"and (@Status is null or Status = @Status)" +
                                 $"and (@Code is null or Code like @Code)" +
                                 $"and (@Name is null or Name like @Name)" +
                                 $"and (@CategoryId is null or CategoryId in (@CategoryId))";
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

            command.Parameters.Add("@FromCreatedDate", MySqlDbType.DateTime).Value = request.FromCreatedDate;
            command.Parameters.Add("@ToCreatedDate", MySqlDbType.DateTime).Value = request.ToCreatedDate;
            command.Parameters.Add("@FromUpdatedDate", MySqlDbType.DateTime).Value = request.FromUpdatedDate;
            command.Parameters.Add("@ToUpdatedDate", MySqlDbType.DateTime).Value = request.ToUpdatedDate;
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = request.Status;
            command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = request.Name;
            command.Parameters.Add("@Code", MySqlDbType.VarChar).Value = request.Code;
            command.Parameters.Add("@CategoryId", MySqlDbType.VarString).Value =
                request.CategoryId is null || !request.CategoryId.Any() 
                    ? null 
                    : string.Join(",", request.CategoryId);

            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                subjects.Add(new Subject()
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Code = DbUtils.SafeGetString(reader, "Code"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    CategoryId = DbUtils.SafeGetString(reader, "CategoryId")
                });
            }
            reader.Close();
            page.Data = subjects;

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