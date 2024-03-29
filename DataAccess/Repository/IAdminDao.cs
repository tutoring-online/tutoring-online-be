﻿using DataAccess.Entities.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Admin;

namespace DataAccess.Repository;

public interface IAdminDao
{
    IEnumerable<Admin?> GetAdmins();

    IEnumerable<Admin?> GetAdminById(string id);

    IEnumerable<Admin?> GetAdminByFirebaseUid(string uid);

    int CreateAdmin(Admin admin);
    void UpdateAdmin(Admin admin, string id);
    int DeleteAdmin(string id);
    Admin? getAdminByEmail(string email);
}
