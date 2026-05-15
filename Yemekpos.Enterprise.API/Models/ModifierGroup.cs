using System.Collections.Generic;

namespace Yemekpos.Enterprise.API.Models
{
    public class ModifierGroup
    {
        public int Id { get; set; }
        
        // Tam 4 Dil Desteği
        public string NameTr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameRu { get; set; } = string.Empty;
        
        // İş Kuralları
        public int MinSelect { get; set; } 
        public int MaxSelect { get; set; } 

        public ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
        public ICollection<ProductModifierGroup> ProductModifierGroups { get; set; } = new List<ProductModifierGroup>();
    }
}