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

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate";
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

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate";
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

            var selectStatement = "Select Id, SubjectId, TotalLessons, Description, Price, Name, Status, CreatedDate, UpdatedDate";
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
}
