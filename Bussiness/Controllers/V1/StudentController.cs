using DataAccess.Models.Student;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;
[ApiController]
[Route("/api/v1/students")]
public class StudentController
{
    private readonly IStudentService studentService;

    public StudentController(IStudentService studentService)
    {
        this.studentService = studentService;
    }

    [HttpGet]
    public IEnumerable<StudentDto> GetStudents()
    {
        return studentService.GetStudents();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<StudentDto> GetStudent(string id)
    {
        var students = studentService.GetStudentById(id);

        return students;
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateStudent(string id, UpdateStudentDto updateStudentDto)
    {
        var students = studentService.GetStudentById(id);

        if (students.Any())
        {
            studentService.UpdateStudent(updateStudentDto.AsEntity(), id);
        }
    }
}
