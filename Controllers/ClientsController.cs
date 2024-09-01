using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpringBootCloneApp.Models.DTOs;
using SpringBootCloneApp.Data;
using SpringBootCloneApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SpringBootCloneApp.Models.Enums;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using SpringBootCloneApp.Controllers.RequestModels;

namespace SpringBootCloneApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Client> _userManager;
        public ClientController(AppDbContext context, UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }       

        // GET: api/Clients/profile
        [HttpGet("profile")]
        public async Task<ActionResult<ClientDTO>> GetClientInfo()
        {
            var strId = User.Claims.First(x=> x.Type == ClaimTypes.NameIdentifier).Value;
            long longId;


            if (!long.TryParse(strId, out longId))
                return NotFound();

            var client = await _context.Clients.FirstOrDefaultAsync(x=> x.Id == longId);

            if (client == null)
            {
                return NotFound();
            }

            return new ClientDTO(client);
        }


        [HttpPut("")]
        public async Task<IActionResult> UpdateClient(ClientDTO incomingClient)
        {
            var strId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            long longId;


            if (!long.TryParse(strId, out longId))
                return BadRequest();

            if (longId != incomingClient.Id)
            {
                return BadRequest();
            }

            var client = await _context.Clients.FindAsync(longId);

            if (client == null)
            {
                return NotFound();
            }

            client.UpdateClientFromDTO(incomingClient); // no password

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Conflict();
            }

            return NoContent();
        }

        // PUT: api/Clients/password/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("changePassword")]
        public async Task<IActionResult> UpdatePassword(PasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var strId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            long longId;


            if (!long.TryParse(strId, out longId))
                return BadRequest();

            if (longId != model.Id)
            {
                return BadRequest();
            }

            var client = await _userManager.FindByIdAsync(longId.ToString());

            if (client == null)
            {
                return NotFound();
            }

            var res = await _userManager.ChangePasswordAsync(client, model.OldPassword, model.NewPassword);

            if (!res.Succeeded)
                return BadRequest(res.Errors);

            return Accepted();
        }

    }
}
