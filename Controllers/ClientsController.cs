using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCorePostgres.Models.DTOs;
using EFCorePostgres.Data;
using EFCorePostgres.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EFCorePostgres.Models.Enums;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace EFCorePostgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/profile
        [HttpGet("profile")]
        [Authorize]
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


        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(long id, ClientDTO incomingClient)
        {
            if (id != incomingClient.Id)
            {
                return BadRequest();
            }

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            client.UpdateClientFromDTO(incomingClient);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ClientExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Clients/password/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("password/{id}")]
        public async Task<IActionResult> UpdatePassword(long id, ClientDTO incomingClient)
        {
            if (id != incomingClient.Id)
            {
                return BadRequest();
            }

            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            client.PasswordHash = incomingClient.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ClientExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(ClientDTO clientDto)
        {
            var client = new Client(clientDto);
            _context.Clients.Add(client);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message);
            }

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(long id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
