using DataAccess.Entities.Subject;
using DataAccess.Utils;
using MySql.Data.MySqlClient;

namespace DataAccess.Repository.MySqlRepository;

public class SubjectDao : ISubjectDao
{
    public IEnumerable<Subject?> GetSubjects()
    {
        var subjects= new List<Subject?>();

        try
        {
            using var connection = new MySqlConnection(DbUtils.GetDbConnection());
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "Select * From Subject";

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                subjects.Add(new Subject
                {
                    Id = reader.GetString("Id"),
                    SubjectCode = reader.GetString("SubjectCode"),
                    Name = reader.GetString("Name"),
                    Description = reader.GetString("Description"),
                    Status = reader.GetString("Status"),
                    CreatedDate = reader.GetMySqlDateTime("CreatedDate").ToString(),
                    UpdatedDate = reader.GetMySqlDateTime("UpdatedDate").ToString(),
                    CategoryId = reader.GetString("CategoryId")
                });
                
            }
            Console.WriteLine("Open connection");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Console.WriteLine("Closed connection");
        }

        return subjects;
    }
}