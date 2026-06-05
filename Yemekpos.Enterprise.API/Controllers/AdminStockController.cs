using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Yemekpos.Enterprise.API.Models; // Kendi model yoluna göre kontrol et
using Yemekpos.Enterprise.API.Data;
namespace Yemekpos.Enterprise.API.Controllers
{
    [Route("api/admin/stock")]
    [ApiController]
    public class AdminStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminStockController(AppDbContext context)
        {
            _context = context;
        }

        // Yöneticinin Next.js ekranından bize göndereceği verinin kalıbı
        public class StockCountDto
        {
            public int ProductId { get; set; }
            public int NewBalance { get; set; } // Depoda fiziksel olarak sayılan güncel rakam
        }

        [HttpPost("count")]
        public async Task<IActionResult> UpdateStockBalance([FromBody] StockCountDto request)
        {
            try
            {
                // 1. Ürünü veritabanından bul
                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null) 
                    return NotFound("Ürün bulunamadı.");

                // 2. FARK HESABI: Sistemdeki rakam ile sayılan rakam arasındaki farkı bul
                // Örn: Sistemde 10 var, Depocu 50 saydı. Fark: +40 adet stok girişi olmalı.
                int difference = request.NewBalance - product.StockBalance;

                // Eğer fark yoksa (zaten doğru sayılmışsa) boşuna işlem yapma
                if (difference == 0)
                    return Ok(new { message = "Stok zaten güncel, hareket kaydedilmedi." });

                // 3. Ürünün bakiyesini yöneticinin yeni saydığı rakama eşitle
                product.StockBalance = request.NewBalance;

                // 4. LOGLAMA: Bu sayım işlemini hareket tablosuna kaydet
                var movement = new StockMovement
                {
                    ProductId = product.Id,
                    Quantity = difference, // Aradaki fark kadar giriş (veya eksikse çıkış)
                    Type = StockMovementType.Counting, // Tip: Sayım/Düzeltme
                    CreatedAt = DateTime.UtcNow
                };

                _context.StockMovements.Add(movement);
                await _context.SaveChangesAsync();

                return Ok(new 
                { 
                    message = "Sayım başarıyla kaydedildi.", 
                    newBalance = product.StockBalance,
                    movementApplied = difference
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Sayım kaydedilirken hata oluştu: " + ex.Message);
            }
        }
    }
}