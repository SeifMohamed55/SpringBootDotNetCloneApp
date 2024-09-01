using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpringBootCloneApp.Data;
using System.Runtime.InteropServices;
using System;
using Microsoft.AspNetCore.Authorization;
using SpringBootCloneApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using SpringBootCloneApp.Models.Enums;
using SpringBootCloneApp.Models;
using Microsoft.AspNetCore.Identity;
using SpringBootCloneApp.Controllers.RequestModels;
using System.Net;
using SpringBootCloneApp.Controllers.ResponseModels;
using SpringBootCloneApp.Services;

namespace SpringBootCloneApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Client> _userManager;

        public AdminController(AppDbContext context, UserManager<Client> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet("allClients")]
        public IActionResult GetClients()
        {
            var clients = _context.Clients.Select(x => new ClientDTO(x));

            return Ok(clients);
        }


        [HttpGet("allAdmins")]
        public IActionResult GetAdmins()
        {
            var admins = _context.Clients
                .Include(x => x.Authorities)
                .Where(x => x.Authorities.Select(x => x.Id).Contains((long)Role.ROLE_ADMIN))
                .Select(x => new ClientDTO(x));

            return Ok(admins);
        }



        [HttpPost("registration")]
        public async Task<IActionResult> RegisterAdmin(RegisterCustomRequest model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Client()
            {
                Email = model.Email,
                Address = model.Address,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
            };


            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user,
                                                new[] { Role.ROLE_USER.ToString(), Role.ROLE_ADMIN.ToString() });

                if (result.Succeeded)
                    return CreatedAtAction(nameof(GetUserById), new { result = "User created successfully", user = new ClientDTO(user) });
            }

            return BadRequest(result.Errors);

        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            var client = await _userManager.FindByIdAsync(id.ToString());
            if (client == null)
                return NotFound();

            client.Banned = true;
            var res = await _userManager.UpdateAsync(client);
            if (!res.Succeeded)
                return Problem(detail: "User Couldn't be deleted");

            return Ok();

        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpGet("email/{email}")]
        public IActionResult GetClientsByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpPost("addFood")]
        public async Task<IActionResult> AddFood(FoodDTO foodDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var foodTags = foodDTO.FoodTags.Select(tag => new FoodTag()
            {
                Tag = tag
            }).ToList();


            var foodOrigins = foodDTO.FoodOrigins.Select(origin => new FoodOrigin()
            {
                Origin = origin
            }).ToList();


            var newFood = new Food()
            {
                CookTime = foodDTO.CookTime,
                FoodTags = foodTags,
                FoodOrigins = foodOrigins,
                Hidden = foodDTO.Hidden,
                ImageUrl = foodDTO.ImageUrl,
                Name = foodDTO.Name,
                Price = foodDTO.Price.Value
            };

            await _context.Foods.AddAsync(newFood);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Problem("Couldn't be added");
            }

            return Created();

        }


        [HttpPost("hideFood")]
        public async Task<IActionResult> HideFood(long id)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == id);
            if (food is null)
                return BadRequest();

            food.Hidden = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Problem("Couldn't be added");
            }

            return Created();

        }




        [HttpPost("unhideFood")]
        public async Task<IActionResult> UnhideFood(long id)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == id);
            if (food is null)
                return BadRequest();

            food.Hidden = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Problem("Couldn't be added");
            }

            return Created();

        }


        [HttpPut("updateFood")]
        public async Task<IActionResult> updateFood(FoodDTO foodDTO)
        {
            if (!ModelState.IsValid || foodDTO.Id == null)
                return BadRequest();


            var dbFood = await _context.Foods.FirstOrDefaultAsync(x => x.Id == foodDTO.Id);
            if (dbFood is null)
                return NotFound();

            dbFood.UpdateFromDTO(foodDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Problem("Couldn't be added");
            }

            return Accepted(foodDTO);
        }



        [HttpGet("allFood")]
        public async Task<IActionResult> GetPageOfFood(int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodTags)
                .Include(x => x.FoodOrigins)
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));

        }
    }
}
