namespace Yemekpos.Enterprise.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        // Foreign Key (Hangi Kategoriye Ait?)
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Çoklu Dil Desteği
        public string NameTr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameRu { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        // Mevcut özelliklerin altına bunu ekliyoruz
public ICollection<ProductModifierGroup> ProductModifierGroups { get; set; } = new List<ProductModifierGroup>();
    }
}