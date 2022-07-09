using Anotar.NLog;
using DataAccess.Entities.Student;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.MySqlRepository;

public class StudentDao:IStudentDao
{
    private readonly ILogger<StudentDao> logger;

    public StudentDao(ILogger<StudentDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Student?> GetStudents()
    {
        var student = new List<Student?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Email, Name, Grade, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Student";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                student.Add(new Student
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Grade = DbUtils.SafeGetInt16(reader, "Grade"),
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

        return student;
    }
    public IEnumerable<Student?> GetStudentById(string id)
    {
        var students = new List<Student?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Email, Name, Grade, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Student";
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
                students.Add(new Student
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Grade = DbUtils.SafeGetInt16(reader, "Grade"),
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

        return students;
    }

    public IEnumerable<Student?> GetStudentByFirebaseUid(string uid)
    {
        var students = new List<Student?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@uid";

            var selectStatement = "Select Id, Email, Name, Grade, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Student";
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
                students.Add(new Student
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Email = DbUtils.SafeGetString(reader, "Email"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Grade = DbUtils.SafeGetInt16(reader, "Grade"),
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
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return students;
    }

    public int CreateStudent(Student student)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            IEnumerable<Student> students = new[]
            {
                student
            };

            using var command = MySqlUtils.CreateInsertStatement(students, connection);
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

    public void UpdateStudent(Student asEntity, string id)
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

    public int DeleteStudent(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Student).Name, connection, id);
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

    public Dictionary<string, Student?> GetStudents(HashSet<string> ids)
    {
        var student = new Dictionary<string, Student?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Email, Name, Grade, Phone, Status, Gender, Birthday, Address, AvatarURL, CreatedDate, UpdatedDate";
            var fromStatement = "From Student";
            var whereSatement = $"Where id In ( {MySqlUtils.CreateInStatementValues(ids)})";
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
                student.Add(
                    DbUtils.SafeGetString(reader, "Id"),
                    new Student { 
                            Id = DbUtils.SafeGetString(reader, "Id"),
                            Email = DbUtils.SafeGetString(reader, "Email"),
                            Name = DbUtils.SafeGetString(reader, "Name"),
                            Grade = DbUtils.SafeGetInt16(reader, "Grade"),
                            Status = DbUtils.SafeGetInt16(reader, "Status"),
                            Phone = DbUtils.SafeGetString(reader, "Phone"),
                            Gender = DbUtils.SafeGetInt16(reader, "Gender"),
                            Address = DbUtils.SafeGetString(reader, "Address"),
                            AvatarURL = DbUtils.SafeGetString(reader, "AvatarURL"),                   
                            Birthday = DbUtils.SafeGetDateTime(reader, "Birthday"),
                            CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                            UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate")
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

        return student;
    }
}
