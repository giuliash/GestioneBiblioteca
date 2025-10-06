using Microsoft.EntityFrameworkCore;
using GestioneBiblioteca.Data;
using TuoProgetto.Data;
using Stripe;
using GestioneBiblioteca.Models;

var builder = WebApplication.CreateBuilder(args);

// ======================
// Configurazione Stripe
// ======================
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// ======================
// DbContext - Due database separati
// ======================
builder.Services.AddDbContext<GestioneBibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestioneBibliotecaContext")
        ?? throw new InvalidOperationException("Connection string 'GestioneBibliotecaContext' not found.")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// ======================
// Session per totale donazioni
// ======================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".GestioneBiblioteca.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ======================
// MVC + Razor Pages
// ======================
builder.Services.AddAntiforgery();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ======================
// CORS
// ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AccessoLimitato", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ======================
// Seed DB Biblioteca
// ======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Errore durante l'inizializzazione del database.");
    }
}

// ======================
// Pipeline HTTP
// ======================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCors("AccessoLimitato");
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Donazioni}/{action=Create}/{id?}");

app.MapRazorPages();

app.Run();