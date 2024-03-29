﻿using Anotar.NLog;
using DataAccess.Entities.Admin;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.MySqlRepository;

public class AdminDao: IAdminDao
{
    private readonly ILogger<AdminDao> logger;

    public AdminDao(ILogger<AdminDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Admin?> GetAdmins()
    {
        var admin = new List<Admin?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Email, Name, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Admin";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                admin.Add(new Admin
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Birthday = DbUtils.SafeGetDateTime(reader, "Birthday"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),                                  
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

        return admin;
    }
    public IEnumerable<Admin?> GetAdminById(string id)
    {
        var admins = new List<Admin?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Email, Name, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Admin";
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
                admins.Add(new Admin
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),
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

        return admins;
    }
    
    
    public IEnumerable<Admin?> GetAdminByFirebaseUid(string uid)
    {
        var admins = new List<Admin?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@uid";

            var selectStatement = "Select Id, Email, Name, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Admin";
            var whereStatement = $"Where Uid = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = uid;
            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                admins.Add(new Admin
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),
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

        return admins;
    }

    public int CreateAdmin(Admin admin)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            IEnumerable<Admin> admins = new[]
            {
                admin
            };

            using var command = MySqlUtils.CreateInsertStatement(admins, connection);
            return command.ExecuteNonQuery();
        }catch (MySqlException e)
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

    public void UpdateAdmin(Admin admin, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(admin, connection, $"id = {id}");
            command.ExecuteNonQuery();
        }catch (MySqlException e)
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

    public int DeleteAdmin(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Admin).Name, connection, id, (int)AdminStatus.Deleted);
            return command.ExecuteNonQuery();
        }catch (MySqlException e)
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

    public Admin? getAdminByEmail(string email)
    {
        Admin admin = null;

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@email";

            var selectStatement = "Select Id, Email, Name, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Admin";
            var whereStatement = $"Where Email = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = email;
            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                admin = new Admin
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Status = DbUtils.SafeGetInt16(reader, "Status"),
                    Phone = DbUtils.SafeGetString(reader, "Phone"),
                    Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                    Address = DbUtils.SafeGetString(reader, "Address"),
                    AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),
                    Birthday = DbUtils.SafeGetDateTime(reader, "Birthday"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate")
                };

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

        return admin;
    }
}
