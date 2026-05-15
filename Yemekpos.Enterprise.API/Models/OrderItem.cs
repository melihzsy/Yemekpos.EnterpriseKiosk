namespace Yemekpos.Enterprise.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        // Hangi Siparişe Ait?
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Hangi Ürün Alındı?
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Ürünün o anki fiyatı (Gelecekte değişse bile fiş bozulmasın diye)
    }
}