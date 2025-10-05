using Microsoft.EntityFrameworkCore;
using GestioneBiblioteca.Data;
using TuoProgetto.Data;
using Stripe;
using GestioneBiblioteca.Models;

var builder = WebApplication.CreateBuilder(args);


StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


builder.Services.AddDbContext<GestioneBibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));


builder.Services.AddDistributedMemoryCache();


builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".GestioneBiblioteca.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout di 30 minuti
    options.Cookie.HttpOnly = true; // Sicurezza: previene accesso da JavaScript
    options.Cookie.IsEssential = true; // Necessario per GDPR
});


builder.Services.AddAntiforgery();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();              // 1. PRIMA Routing

app.UseSession();              // 2. POI Session (DOPO UseRouting)

app.UseCors("AccessoLimitato");

app.UseAuthorization();        // 3. INFINE Authorization (DOPO UseSession)


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Donazioni}/{action=Create}/{id?}");

app.MapRazorPages(); // Aggiunto per supportare Razor Pages

app.Run();