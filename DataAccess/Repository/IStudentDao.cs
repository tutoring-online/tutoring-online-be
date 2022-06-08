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

}
