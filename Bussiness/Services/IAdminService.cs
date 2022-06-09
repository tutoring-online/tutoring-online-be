using DataAccess.Entities.Admin;
using DataAccess.Models.Admin;
using FirebaseAdmin.Auth;

namespace tutoring_online_be.Services;

public interface IAdminService
{
    IEnumerable<AdminDto> GetAdmins();

    IEnumerable<AdminDto> GetAdminById(string id);
    
    IEnumerable<AdminDto> GetAdminByFirebaseUid(string uid);

    int CreateAdminByFirebaseToken(FirebaseToken token);
    void UpdateAdmin(Admin asEntity, string id);
    int DeleteAdmin(string id);
}
