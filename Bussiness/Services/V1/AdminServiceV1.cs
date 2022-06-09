using DataAccess.Entities.Admin;
using DataAccess.Models.Admin;
using DataAccess.Repository;
using DataAccess.Utils;
using FirebaseAdmin.Auth;

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

    public IEnumerable<AdminDto> GetAdminByFirebaseUid(string uid)
    {
        return adminDao.GetAdminByFirebaseUid(uid).Select(admin => admin.AsDto());
    }

    public int CreateAdminByFirebaseToken(FirebaseToken token)
    {
        var userRecord = FirebaseAuth.DefaultInstance.GetUserAsync(token.Uid).Result;

        var admin = new Admin()
        {
            uid = userRecord.Uid,
            Email = userRecord.Email,
            Name = userRecord.DisplayName,
            Phone = userRecord.PhoneNumber,
            Status = 0,
            AvatarURL = userRecord.PhotoUrl,
            CreatedDate = DateTime.Now
        };

        return adminDao.CreateAdmin(admin);
    }

    public void UpdateAdmin(Admin admin, string id)
    {
        adminDao.UpdateAdmin(admin, id);
    }

    public int DeleteAdmin(string id)
    {
        return adminDao.DeleteAdmin(id);
    }
}
