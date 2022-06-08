using Anotar.NLog;
using DataAccess.Entities.Tutor;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.MySqlRepository;

public class TutorDao : ITutorDao
{
    private readonly ILogger<TutorDao> logger;

    public TutorDao(ILogger<TutorDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Tutor?> GetTutors()
    {
        var tutor = new List<Tutor?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Email, Name, MeetingUrl, Phone, Status, Gender, Birthday, Address, AvatarURL, Description, CreatedDate, UpdatedDate";
            var fromStatement = "From Tutor";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tutor.Add(new Tutor
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    MeetingUrl = DbUtils.SafeGetString(reader, "MeetingUrl"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Birthday = DbUtils.SafeGetDateTime(reader, "Birthday"),
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

        return tutor;
    }
    public IEnumerable<Tutor?> GetTutorById(string id)
    {
        var tutors = new List<Tutor?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Email, Name, MeetingUrl, Phone, Status, Gender, Birthday, Address, AvatarURL, Description, CreatedDate, UpdatedDate";
            var fromStatement = "From Tutor";
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
                tutors.Add(new Tutor
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    MeetingUrl = DbUtils.SafeGetString(reader, "MeetingUrl"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    Birthday = DbUtils.SafeGetDateTime(reader, "Birthday"),
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

        return tutors;
    }


}
