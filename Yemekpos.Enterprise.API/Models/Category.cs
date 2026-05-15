using System.Collections.Generic;

namespace Yemekpos.Enterprise.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        // Çoklu Dil Desteği
        public string NameTr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameRu { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;

        // İlişki (Navigation Property): Bir kategorinin birden fazla ürünü olur
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}