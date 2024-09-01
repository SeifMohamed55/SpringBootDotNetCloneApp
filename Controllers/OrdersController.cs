using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpringBootCloneApp.Controllers.RequestModels;
using SpringBootCloneApp.Data;
using SpringBootCloneApp.Models;

namespace SpringBootCloneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodOrder>>> GetFoodOrders()
        {
            return await _context.FoodOrders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodOrder>> GetFoodOrder(long id)
        {
            var foodOrder = await _context.FoodOrders.FindAsync(id);

            if (foodOrder == null)
            {
                return NotFound();
            }

            return foodOrder;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*    [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodOrder(long id, OrderRequest foodOrder)
        {
            if (id != foodOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<FoodOrder>> PostFoodOrder(OrderRequest foodOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var latLang = new LatLng()
            {
                Lat = foodOrder.Lat,
                Lng = foodOrder.Lng
            };

            var order = new FoodOrder()
            {
                Address = foodOrder.Address,
                ClientId = foodOrder.ClientId.Value,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Name = foodOrder.Name,
                TotalPrice = foodOrder.TotalPrice,
                Status = Models.Enums.OrderStatus.NEW,
                LatLng = latLang,
                UpdatedAt = DateOnly.FromDateTime(DateTime.Now),
                OrderItems = foodOrder.OrderItems

            };
            _context.FoodOrders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodOrder", new { id = foodOrder.Id }, foodOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodOrder(long id)
        {
            var foodOrder = await _context.FoodOrders.FindAsync(id);
            if (foodOrder == null)
            {
                return NotFound();
            }

            _context.FoodOrders.Remove(foodOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodOrderExists(long id)
        {
            return _context.FoodOrders.Any(e => e.Id == id);
        }
    }
}
