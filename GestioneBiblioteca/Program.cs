using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestioneBiblioteca.Data;
using GestioneBiblioteca.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Aggiunta per permettere connessioni da altri dispositivi
builder.WebHost.UseUrls("http://0.0.0.0:5000");


builder.Services.AddDbContext<GestioneBibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestioneBibliotecaContext")
        ?? throw new InvalidOperationException("Connection string 'GestioneBibliotecaContext' not found.")));

builder.Services.AddAntiforgery();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Accesso limitato", policy =>
    {
        policy.WithOrigins("il_mio_pc")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAntiforgery();

app.UseRouting();

app.UseCors("AccessoLimitato");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Libri}/{action=Index}/{id?}");

app.Run();
