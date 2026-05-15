namespace Yemekpos.Enterprise.API.Models
{
    public class Modifier
    {
        public int Id { get; set; }
        public int ModifierGroupId { get; set; }
        public ModifierGroup ModifierGroup { get; set; } = null!;

        // Tam 4 Dil Desteği
        public string NameTr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameRu { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public decimal ExtraPrice { get; set; } // +20 TL, +0 TL vb.
        public bool IsActive { get; set; } = true;
    }
}