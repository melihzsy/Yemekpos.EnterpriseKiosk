using System;
using System.Collections.Generic;

namespace Yemekpos.Enterprise.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty; // Örn: #042
        
        public string OrderType { get; set; } = string.Empty; // "dine-in" veya "takeaway"
        public string PaymentMethod { get; set; } = string.Empty; // "card" veya "cash"
        
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişki: Bir siparişin içinde birden fazla ürün kalemi olabilir
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}