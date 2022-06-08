using DataAccess.Models.Student;
using DataAccess.Repository;
using DataAccess.Utils;

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
}
