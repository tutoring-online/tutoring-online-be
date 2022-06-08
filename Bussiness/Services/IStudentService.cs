using DataAccess.Models.Student;

namespace tutoring_online_be.Services;

public interface IStudentService
{
    IEnumerable<StudentDto> GetStudents();
    IEnumerable<StudentDto> GetStudentById(string id);
}
