using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.Tutor;

public class Tutor
{
    public string? Id { get; init; }
    public string? Email { get; init; }
    public string? Name { get; init; }
    public string? MeetingUrl { get; init; }
    public string? Phone { get; init; }
    public int? Status { get; set; }
    public int? Gender { get; init; }
    public DateTime? Birthday { get; init; }
    public string? Address { get; init; }
    public string? AvatarURL { get; init; }
    public string? Description { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public string? Uid { get; set; }
}
