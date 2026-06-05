using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yemekpos.Enterprise.API.Models; 
using Yemekpos.Enterprise.API.Data;

namespace Yemekpos.Enterprise.API.Controllers
{
    // 1. DTO: Fiyat ve İsim Güncelleme
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string NameTr { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }

    // 2. DTO: Stok Güncelleme
    public class ProductStockUpdateDto
    {
        public int Id { get; set; }
        public int StockBalance { get; set; } 
    }

    // 3. YENİ DTO: Kiosk'tan gelen sepet öğeleri
    public class CheckoutItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tüm ürünleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // PUT: Fiyat ve detay güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest("Güvenlik ihlali: Ürün ID'leri eşleşmiyor.");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Güncellenmek istenen ürün bulunamadı.");

            product.NameTr = updatedProduct.NameTr; 
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Ürün başarıyla güncellendi!", updatedData = product });
        }

        // PUT: Sadece stok güncelle (Yönetici Paneli)
        [HttpPut("stock/{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] ProductStockUpdateDto stockUpdate)
        {
            if (id != stockUpdate.Id)
                return BadRequest("Güvenlik ihlali: Ürün ID'leri eşleşmiyor.");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Stok işlemi için ürün bulunamadı.");

            product.StockBalance = stockUpdate.StockBalance;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Stok başarıyla güncellendi!", currentStock = product.StockBalance });
        }

        // YENİ POST: Kiosk'tan sipariş geldiğinde stokları güvenli bir şekilde düşen metod (Transaction)
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] List<CheckoutItemDto> cartItems)
        {
            if (cartItems == null || cartItems.Count == 0)
                return BadRequest("Sepet boş olamaz.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in cartItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null)
                        return NotFound($"ID'si {item.ProductId} olan ürün bulunamadı.");

                    if (product.StockBalance < item.Quantity)
                        return BadRequest($"Sipariş iptal edildi: {product.NameTr} için yeterli stok yok! Kalan: {product.StockBalance}");

                    product.StockBalance -= item.Quantity;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Sipariş başarıyla alındı ve stoklar hatasız güncellendi!" });
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "İşlem sırasında kritik bir hata oluştu: " + ex.Message);
            }
        }
    }
}