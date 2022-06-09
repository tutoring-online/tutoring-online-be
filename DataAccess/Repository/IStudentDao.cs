using DataAccess.Entities.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IStudentDao
{
    IEnumerable<Student?> GetStudents();

    IEnumerable<Student?> GetStudentById(string id);

    public IEnumerable<Student?> GetStudentByFirebaseUid(string uid);

    public int CreateStudent(Student student);

    void UpdateStudent(Student asEntity, string id);
    int DeleteStudent(string id);
}
