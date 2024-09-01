using SpringBootCloneApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpringBootCloneApp.Services;
using SpringBootCloneApp.Models;
using Microsoft.EntityFrameworkCore;
using SpringBootCloneApp.Models.DTOs;
using SpringBootCloneApp.Controllers.ResponseModels;

namespace SpringBootCloneApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet("{index}")]
        public async Task<IActionResult> getPageOfFood(int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodTags)
                .Include(x => x.FoodOrigins)
                .Where(x => x.Hidden == false)
                .OrderBy(x=> x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (index > paginatedList.TotalPages)
                return BadRequest();
            
            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));

        }


        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> getFoodsBySearching(string searchTerm, int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodTags)
                .Include(x => x.FoodOrigins)
                .Where(x => x.Hidden == false)
                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));
            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (paginatedList.TotalPages == 0)
                return NotFound();

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));
        }


        [HttpGet("tag/{tagName}")]
        public async Task<IActionResult> getFoodsByTag(string tagName, int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodOrigins)
                .Include(x => x.FoodTags)
                .Where(x => x.FoodTags.Select(x => x.Tag.ToLower()).Contains(tagName.ToLower()))
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (paginatedList.TotalPages == 0)
                return NotFound();

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));
        }

        [HttpGet("search/tags")]
        public async Task<IActionResult> getFoodsByTags([FromQuery] List<string> userTags, int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodTags)
                .Include(x => x.FoodOrigins)
                .Where(x => x.Hidden == false)
                .Where(x => userTags.All(z => x.FoodTags.Select(x => x.Tag.ToLower()).Contains( z.ToLower())))
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (paginatedList.TotalPages == 0)
                return NotFound();

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));
        }

        [HttpGet("tags")]
        public async Task<IActionResult> getAllTags()
        {

            return Ok(await _context.FoodTags.Select(x => x.Tag).Distinct().ToListAsync());
        }




        [HttpGet("origin/{originName}")]
        public async Task<IActionResult> getFoodsByOrigin(string originName, int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodOrigins)
                .Include(x => x.FoodTags)
                .Where(x => x.FoodOrigins.Select(x => x.Origin.ToLower()).Contains(originName.ToLower()))
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (paginatedList.TotalPages == 0)
                return NotFound();

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));
        }


        [HttpGet("search/origins")]
        public async Task<IActionResult> getFoodsByOrigins([FromQuery] List<string> userOrigins, int index = 1)
        {
            if (index <= 0)
                return BadRequest();

            var foods = _context.Foods
                .Include(x => x.FoodOrigins)
                .Where(x => x.Hidden == false)
                .Where(food => userOrigins.All(userOrigin => food.FoodOrigins.Select(x => x.Origin.ToLower()).Contains(userOrigin.ToLower())))
                .OrderBy(x => x.Name)
                .Select(x => new FoodDTO(x));

            var paginatedList = await PaginatedList<FoodDTO>.CreateAsync(foods, index);

            if (paginatedList.TotalPages == 0)
                return NotFound();

            if (index > paginatedList.TotalPages)
                return BadRequest();

            return Ok(new PaginatedResponse<FoodDTO>(paginatedList));
        }


        [HttpGet("id/{foodId}")]
        public async Task<IActionResult> getFoodById(int foodId)
        {
            var food = await _context.Foods
                .Include(x => x.FoodTags)
                .Include(x => x.FoodOrigins)
                .Select(x => new FoodDTO(x))
                .FirstOrDefaultAsync(x => x.Id == foodId);

            if (food is null)
                return NotFound();

            return Ok(food);

        }
    }
}
