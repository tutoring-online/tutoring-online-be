using DataAccess.Models.Student;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services;

public interface IStudentService
{
    IEnumerable<StudentDto> GetStudents();
    IEnumerable<StudentDto> GetStudentById(string id);
    IEnumerable<StudentDto> GetStudentByFirebaseUid(string uid);
    int CreateStudentByFirebaseToken(FirebaseToken token);
}
