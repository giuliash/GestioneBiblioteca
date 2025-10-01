using GestioneBiblioteca.Models;
using Microsoft.EntityFrameworkCore;
using GestioneBiblioteca.Models;

namespace TuoProgetto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Donazione> Donazioni { get; set; }
    }
}
