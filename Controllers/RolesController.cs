using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EFCorePostgres.Models;
using EFCorePostgres.Models.Enums;

namespace EFCorePostgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Authority> _roleManager;

        public RolesController(RoleManager<Authority> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("add")]        
        public async Task<IActionResult> AddRole(string roleName)
        {

           var x =  await _roleManager.CreateAsync(new Authority()
            {
                Name = roleName
            });

            if (x.Succeeded) return CreatedAtAction("Adding Role", roleName);

            else return Problem();
        }
    }
}
