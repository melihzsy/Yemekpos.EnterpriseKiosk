using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Yemekpos.Enterprise.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. VERİTABANI BAĞLANTISI (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. JSON DÖNGÜ ENGELLEYİCİ (Kritik!)
// Kategoriler ve Ürünler arasındaki ilişkisel döngünün (Circular Reference) API'yi kilitlemesini önler.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// 3. CORS POLİTİKASI (Next.js Erişimi)
// 3000 portunda çalışan frontend'in backend verilerine güvenle ulaşmasını sağlar.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowKioskApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. VERİTABANI TOHUMLAMA (DATABASE SEEDING)
// Uygulama her çalıştığında veritabanını kontrol eder, boşsa devasa veri setini otomatik basar.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Veritabanının fiziksel olarak var olduğundan emin olur
        context.Database.Migrate(); 
        // Verileri içeri aktarır
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatılırken bir hata oluştu.");
    }
}

// Geliştirme aşamasında Swagger arayüzünü aktif eder
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. ARA KATMAN (MIDDLEWARE) AYARLARI
app.UseHttpsRedirection();

// CORS politikasını etkinleştir (Sıralama önemlidir, Authorization'dan önce gelmeli)
app.UseCors("AllowKioskApp");

app.UseAuthorization();

app.MapControllers();

app.Run();