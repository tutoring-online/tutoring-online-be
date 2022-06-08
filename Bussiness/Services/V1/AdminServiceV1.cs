using DataAccess.Models.Admin;
using DataAccess.Repository;
using DataAccess.Utils;

namespace tutoring_online_be.Services.V1;

public class AdminServiceV1:IAdminService
{
    private readonly IAdminDao adminDao;

    public AdminServiceV1(IAdminDao adminDao)
    {
        this.adminDao = adminDao;
    }

    public IEnumerable<AdminDto> GetAdmins()
    {
        return adminDao.GetAdmins().Select(admin => admin.AsDto());
    }

    public IEnumerable<AdminDto> GetAdminById(string id)
    {
        return adminDao.GetAdminById(id).Select(admin => admin.AsDto());
    }
}
