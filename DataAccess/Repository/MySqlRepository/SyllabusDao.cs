using DataAccess.Entities.Syllabus;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using DataAccess.Models;
using DataAccess.Models.Syllabus;

namespace DataAccess.Repository.MySqlRepository;

public class SyllabusDao: ISyllabusDao
{
    public IEnumerable<Syllabus?> GetSyllabuses()
    {
        var syllabuses = new List<Syllabus?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate, ImageUrl, VideoUrl";
            var fromStatement = "From Syllabus";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                syllabuses.Add(new Syllabus
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    ImageUrl = DbUtils.SafeGetString(reader, "ImageUrl"),
                    VideoUrl = DbUtils.SafeGetString(reader, "VideoUrl"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price")
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

        return syllabuses;
    }

    public IEnumerable<Syllabus?> GetSyllabusById(string id)
    {
        var syllabus = new List<Syllabus?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate, ImageUrl, VideoUrl";
            var fromStatement = "From Syllabus";
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
                syllabus.Add(new Syllabus
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price"),
                    ImageUrl = DbUtils.SafeGetString(reader, "ImageUrl"),
                    VideoUrl = DbUtils.SafeGetString(reader, "VideoUrl"),
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

        return syllabus;
    }

    public void CreateSyllabuses(IEnumerable<Syllabus> syllabuses)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            using var command = MySqlUtils.CreateInsertStatement(syllabuses, connection);

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

    public void UpdateSyllabus(Syllabus asEntity, string id)
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

    public Dictionary<string, Syllabus> GetSyllabuses(HashSet<string> ids)
    {
        var syllabuses = new Dictionary<string, Syllabus>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate, ImageUrl, VideoUrl";
            var fromStatement = "From Syllabus";
            var whereStatement = $"Where id in ({MySqlUtils.CreateInStatementValues(ids)})";
            var query = MySqlUtils.ConstructQueryByStatements(new List<string> {
                    selectStatement,
                    fromStatement,
                    whereStatement
                });

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                syllabuses.Add(DbUtils.SafeGetString(reader, "Id"), new Syllabus
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price"),
                    ImageUrl = DbUtils.SafeGetString(reader, "ImageUrl"),
                    VideoUrl = DbUtils.SafeGetString(reader, "VideoUrl"),
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

        return syllabuses;
    }

    public Page<Syllabus> GetSyllabuses(int? limit, int? offSet, List<Tuple<string, string>> orderByParams, SearchSyllabusRequest request, bool isNotPaging)
    {
        var page = new Page<Syllabus>();
        page.Pagination = new PageDetail();
        var syllabuses = new List<Syllabus?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate, ImageUrl, VideoUrl";
            var selectCountStatement = "Select count(id) as TotalElement";
            var fromStatement = "From Syllabus";
            var whereStatement = $"Where (@SubjectId is null or SubjectId in (@SubjectId))" +
                                 $"and (@FromCreatedDate is null or CreatedDate >= @FromCreatedDate)" +
                                 $"and (@ToCreatedDate is null or CreatedDate <= @ToCreatedDate)" +
                                 $"and (@FromUpdatedDate is null or UpdatedDate >= @FromUpdatedDate)" +
                                 $"and (@ToUpdatedDate is null or UpdatedDate <= @ToUpdatedDate)" +
                                 $"and (@FromPrice is null or Price >= @FromPrice)" +
                                 $"and (@ToPrice is null or Price <= @ToPrice) " +
                                 $"and (@Status is null or Status = @Status)" +
                                 $"and (@Name is null or Name like @Name)" +
                                 $"and (@FromTotalLessons is null or TotalLessons >= @FromTotalLessons)" +
                                 $"and (@ToTotalLessons is null or TotalLessons <= @ToTotalLessons)";
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

            command.Parameters.Add("@SubjectId", MySqlDbType.VarString).Value = request.SubjectId is null || !request.SubjectId.Any() 
                ? null 
                : string.Join(",", request.SubjectId);
            command.Parameters.Add("@FromCreatedDate", MySqlDbType.DateTime).Value = request.FromCreatedDate;
            command.Parameters.Add("@ToCreatedDate", MySqlDbType.DateTime).Value = request.ToCreatedDate;
            command.Parameters.Add("@FromUpdatedDate", MySqlDbType.DateTime).Value = request.FromUpdatedDate;
            command.Parameters.Add("@ToUpdatedDate", MySqlDbType.DateTime).Value = request.ToUpdatedDate;
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = request.Status;
            command.Parameters.Add("@FromPrice", MySqlDbType.Double).Value = request.FromPrice;
            command.Parameters.Add("@ToPrice", MySqlDbType.Double).Value = request.ToPrice;
            command.Parameters.Add("@FromTotalLessons", MySqlDbType.Int16).Value = request.FromTotalLessons;
            command.Parameters.Add("@ToTotalLessons", MySqlDbType.Int16).Value = request.ToTotalLessons;
            command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = request.Name;

            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                syllabuses.Add(new Syllabus()
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    SubjectId = DbUtils.SafeGetString(reader, "SubjectId"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    TotalLessons = DbUtils.SafeGetInt16(reader, "TotalLessons"),
                    Price = DbUtils.SafeGetDouble(reader, "Price"),
                    ImageUrl = DbUtils.SafeGetString(reader, "ImageUrl"),
                    VideoUrl = DbUtils.SafeGetString(reader, "VideoUrl"),
                });
            }
            reader.Close();
            page.Data = syllabuses;

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
