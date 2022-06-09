using DataAccess.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository;

public interface IAdminDao
{
    IEnumerable<Admin?> GetAdmins();

    IEnumerable<Admin?> GetAdminById(string id);

    IEnumerable<Admin?> GetAdminByFirebaseUid(string uid);

    int CreateAdmin(Admin admin);
    void updateAdmin(Admin admin, string id);
}
