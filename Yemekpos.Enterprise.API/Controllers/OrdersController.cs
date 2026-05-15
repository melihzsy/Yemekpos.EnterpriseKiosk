using Microsoft.AspNetCore.Mvc;
using Yemekpos.Enterprise.API.Data;
using Yemekpos.Enterprise.API.Models;

namespace Yemekpos.Enterprise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            // Yeni siparişi ve içindeki ürünleri (OrderItems) tek seferde veritabanına kaydeder
            order.CreatedAt = DateTime.UtcNow;
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
        }
    }
}