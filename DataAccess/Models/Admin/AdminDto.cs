using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Admin;

public class AdminDto
{
    public string? Id { get; init; }
    public string? Email { get; init; }
    public string? Name { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; init; }
    public int? Gender { get; init; }
    public string? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
    public string? CreatedDate { get; init; }
    public string? UpdatedDate { get; init; }
    
    public string? Uid { get; init; }
}
