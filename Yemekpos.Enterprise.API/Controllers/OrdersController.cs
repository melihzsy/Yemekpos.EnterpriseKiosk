using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Yemekpos.Enterprise.API.Models; // Kendi model klasörünün yolu
using Yemekpos.Enterprise.API.Data;

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

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            try
            {
                // 1. Siparişin temel oluşturulma tarihini ata ve veritabanına ekle
                order.CreatedAt = DateTime.UtcNow;
                _context.Orders.Add(order);

                // 2. STOK MOTORU: Siparişin içindeki ürünleri (OrderItems) kontrol et
                if (order.OrderItems != null && order.OrderItems.Count > 0)
                {
                    foreach (var item in order.OrderItems)
                    {
                        // Ürünü veritabanından bul
                        var product = await _context.Products.FindAsync(item.ProductId);
                        
                        if (product != null)
                        {
                            // A) Kiosk'tan satılan miktar kadar güncel bakiyeyi düşür
                            product.StockBalance -= item.Quantity;

                            // B) Yöneticinin izleyeceği stok hareketi logunu oluştur (Hareket Tablosu)
                            var movement = new StockMovement
                            {
                                ProductId = product.Id,
                                Quantity = -item.Quantity, // Çıkış olduğu için eksi değer yazıyoruz
                                Type = StockMovementType.Sale, // Tip: Kiosk Satışı
                                CreatedAt = DateTime.UtcNow
                            };
                            
                            // Logu veritabanına ekle
                            _context.StockMovements.Add(movement);
                        }
                    }
                }

                // 3. KAYIT (Transaction): Tüm işlemleri tek seferde güvenle veritabanına yaz
                // Entity Framework Core, SaveChangesAsync çağrıldığında bu işlemleri otomatik
                // olarak bir "Transaction" içine alır. Hata çıkarsa hiçbirini kaydetmez, güvenlidir.
                await _context.SaveChangesAsync();

                // 4. Başarılı sonuç döndür (Frontend'e sipariş numarasını ve detayları ilet)
                return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Olası bir hatada sistemin çökmesini engelle ve hatayı Frontend'e bildir
                return StatusCode(500, "Sipariş işlenirken bir sunucu hatası oluştu: " + ex.Message);
            }
        }
    }
}