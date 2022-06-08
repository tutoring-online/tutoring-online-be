using DataAccess.Models.Admin;

namespace tutoring_online_be.Services;

public interface IAdminService
{
    IEnumerable<AdminDto> GetAdmins();

    IEnumerable<AdminDto> GetAdminById(string id);
}
