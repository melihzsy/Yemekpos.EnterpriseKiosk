using System;

namespace Yemekpos.Enterprise.API.Models
{
    public class StockMovement
    {
        public int Id { get; set; }
        
        // Hangi ürüne ait hareket? (İlişki)
        public int ProductId { get; set; }
        public Product Product { get; set; } 

        // Miktar değişimi (Örn: Sayımda +50, Satışta -2)
        public int Quantity { get; set; } 
        
        // İşlem tipi (Sayım mı, Satış mı?)
        public StockMovementType Type { get; set; }
        
        // Hareketin gerçekleştiği an
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}