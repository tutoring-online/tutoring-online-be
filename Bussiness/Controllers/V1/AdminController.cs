using DataAccess.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/admins")]
public class AdminController 
{
    private readonly IAdminService adminService;

    public AdminController(IAdminService adminService)
    {
        this.adminService = adminService;
    }

    [HttpGet]
    public IEnumerable<AdminDto> GetAdmins()
    {
        return adminService.GetAdmins();
    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<AdminDto> GetAdmin(string id)
    {
        var admins = adminService.GetAdminById(id);

        return admins;
    }
}
