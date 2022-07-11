using DataAccess.Models;
using DataAccess.Models.Admin;
using DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using tutoring_online_be.Services;

namespace tutoring_online_be.Controllers.V1;

[ApiController]
[Route("/api/v1/admins")]
public class AdminController : Controller
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

    [HttpPost]
    public IActionResult CreateAdmin(CreateAdminRequest dto)
    {
        AdminDto? adminDto = adminService.GetAdminByEmail(dto.Email);

        if (adminDto is null)
        {
            AdminDto admin = new AdminDto()
            {
                Email = dto.Email,
                Status = (int)AdminStatus.Active
            };

            adminService.CreateAdmin(admin.AsEntity());

            AdminDto responseData = adminService.GetAdminByEmail(admin.Email);

            return Created(new Uri($"api/v1/admins/{responseData.Id}", UriKind.Relative), responseData);
        }
        
        return BadRequest(new ApiResponse
        {
            ResultCode = (int)ResultCode.UserAlreadyCreated,
            ResultMessage = ResultCode.UserAlreadyCreated.ToString()
        });
    }
    
    [HttpPatch]
    [Route("{id}")]
    public void UpdateAdmin(string id, UpdateAdminDto updateAdminDto)
    {
        var admins = adminService.GetAdminById(id);
        if (admins.Any())
        {
            adminService.UpdateAdmin(updateAdminDto.AsEntity(), id);
        }
    }
    
    [HttpDelete]
    [Route("{id}")]
    public void DeleteAdmin(string id)
    {
            adminService.DeleteAdmin(id);
    }
}
