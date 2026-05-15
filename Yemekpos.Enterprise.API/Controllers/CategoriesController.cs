using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yemekpos.Enterprise.API.Data;

namespace Yemekpos.Enterprise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            // Data Projection (İzdüşüm) - Sadece Frontend'in ihtiyacı olanları temiz bir JSON'a çeviririz.
            // Sonsuz döngüleri (Circular Reference) kökten çözer ve performansı maksimize eder.
            var data = await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new
                {
                    id = c.Id,
                    nameTr = c.NameTr,
                    nameEn = c.NameEn,
                    nameAr = c.NameAr,
                    nameRu = c.NameRu,
                    products = c.Products.Where(p => p.IsActive).Select(p => new
                    {
                        id = p.Id,
                        nameTr = p.NameTr,
                        nameEn = p.NameEn,
                        nameAr = p.NameAr,
                        nameRu = p.NameRu,
                        price = p.Price,
                        imageUrl = p.ImageUrl,
                        
                        // Menü Kuralları ve Seçeneklerini Temizce Paketliyoruz
                        modifiers = p.ProductModifierGroups.Select(pmg => new
                        {
                            groupId = pmg.ModifierGroup.Id,
                            groupNameTr = pmg.ModifierGroup.NameTr,
                            groupNameEn = pmg.ModifierGroup.NameEn,
                            groupNameAr = pmg.ModifierGroup.NameAr,
                            groupNameRu = pmg.ModifierGroup.NameRu,
                            minSelect = pmg.ModifierGroup.MinSelect,
                            maxSelect = pmg.ModifierGroup.MaxSelect,
                            options = pmg.ModifierGroup.Modifiers.Where(m => m.IsActive).Select(m => new
                            {
                                id = m.Id,
                                nameTr = m.NameTr,
                                nameEn = m.NameEn,
                                nameAr = m.NameAr,
                                nameRu = m.NameRu,
                                imageUrl = m.ImageUrl,
                                extraPrice = m.ExtraPrice
                            })
                        })
                    })
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}