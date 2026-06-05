using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Yemekpos.Enterprise.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. VERİTABANI BAĞLANTISI (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. JSON DÖNGÜ ENGELLEYİCİ
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// 3. TEK VE GÜÇLÜ CORS POLİTİKASI (Tüm Çatışmaları Çözer)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. VERİTABANI TOHUMLAMA (DATABASE SEEDING)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); 
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatılırken bir hata oluştu.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. ARA KATMAN (MIDDLEWARE) AYARLARI - SIRALAMA KUSURSUZ HALE GETİRİLDİ
// app.UseHttpsRedirection();

app.UseRouting();

// DİKKAT: CORS tam olarak burada, Routing ile Authorization arasında TEK BİR KERE çağrılmalıdır!
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();