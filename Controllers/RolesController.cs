using FgssrApi.Dtos;
using FgssrApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FgssrApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;


        public RolesController(RoleManager<IdentityRole> roleManager) 
        {
            
            _roleManager = roleManager;
        }


        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {

            var getRoles =_roleManager.Roles.ToList();
            return Ok(getRoles);
        }

       

        [HttpPost]
        [Route("CreateNewRole")]
        public async Task<IActionResult> CreateNewRole([FromBody] RoleDto RVM)
        {
            
            await _roleManager.CreateAsync(new IdentityRole(RVM.RoleName));
            
            return Ok("Role created sucessfull");
        }

    }
}
