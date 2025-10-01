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
// DbContext - ENTRAMBI I CONTEXT REGISTRATI
// ======================
builder.Services.AddDbContext<GestioneBibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

// ======================
// Antiforgery
// ======================
builder.Services.AddAntiforgery();

// ======================
// MVC + Views
// ======================
builder.Services.AddControllersWithViews();

// ======================
// CORS (opzionale)
// ======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AccessoLimitato", policy =>
    {
        policy.WithOrigins("http://localhost:5000") // puoi sostituire con IP o dominio reale
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ======================
// Seed DB - CORRETTO per gestire eccezioni
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

// Antiforgery middleware
app.UseRouting();
app.UseCors("AccessoLimitato");
app.UseAuthorization();

// ======================
// Route default
// ======================
// Imposta Donazioni/Create come pagina di default
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Donazioni}/{action=Create}/{id?}");

app.Run();