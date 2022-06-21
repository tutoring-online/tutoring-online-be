using DataAccess.Entities.Student;
using DataAccess.Models.Student;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services;

public interface IStudentService
{
    IEnumerable<StudentDto> GetStudents();
    IEnumerable<StudentDto> GetStudentById(string id);

    Dictionary<string, StudentDto> GetStudents(HashSet<string> ids);

    IEnumerable<StudentDto> GetStudentByFirebaseUid(string uid);
    int CreateStudentByFirebaseToken(FirebaseToken token);
    void UpdateStudent(Student asEntity, string id);
    int DeleteStudent(string id);
}
