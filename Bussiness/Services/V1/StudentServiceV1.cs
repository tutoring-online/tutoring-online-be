using DataAccess.Entities.Student;
using DataAccess.Models.Student;
using DataAccess.Repository;
using DataAccess.Utils;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services.V1;

public class StudentServiceV1:IStudentService
{
    private readonly IStudentDao studentDao;

    public StudentServiceV1(IStudentDao studentDao)
    {
        this.studentDao = studentDao;
    }

    public IEnumerable<StudentDto> GetStudents()
    {
        return studentDao.GetStudents().Select(student => student.AsDto());
    }

    public IEnumerable<StudentDto> GetStudentById(string id)
    {
        return studentDao.GetStudentById(id).Select(student => student.AsDto());
    }

    public IEnumerable<StudentDto> GetStudentByFirebaseUid(string uid)
    {
        return studentDao.GetStudentByFirebaseUid(uid).Select(student => student.AsDto());
    }

    public int CreateStudentByFirebaseToken(FirebaseToken token)
    {
        var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(token.Uid).Result;

        var student = new Student()
        {
            uid = userRecord.Uid,
            Email = userRecord.Email,
            Name = userRecord.DisplayName,
            Phone = userRecord.PhoneNumber,
            Status = 1,
            AvatarURL = userRecord.PhotoUrl,
            CreatedDate = DateTime.Now
        };

        return studentDao.CreateStudent(student);
    }
}
