using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Student;

public class Student
{
    public string? Id { get; init; }
    public string? Email { get; init; }
    public string? Name { get; init; }
    public int? Grade { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; init; }
    public int? Gender { get; init; }
    public DateTime? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public string? uid { get; set; }
}
