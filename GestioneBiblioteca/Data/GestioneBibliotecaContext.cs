using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestioneBiblioteca.Models;

namespace GestioneBiblioteca.Data
{
    public class GestioneBibliotecaContext : DbContext
    {
        public GestioneBibliotecaContext(DbContextOptions<GestioneBibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<GestioneBiblioteca.Models.Libro> Libro { get; set; } = default!;
        public DbSet<Libro> Libri { get; set; }
        public DbSet<Utenti> Utenti { get; set; }
        public DbSet<Prestito> Prestiti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione per garantire email e numero tessera unici
            modelBuilder.Entity<Utenti>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Utenti>()
                .HasIndex(u => u.NumeroTessera)
                .IsUnique();

            // Configurazione relazioni
            modelBuilder.Entity<Prestito>()
                .HasOne(p => p.Libro)
                .WithMany()
                .HasForeignKey(p => p.LibroId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestito>()
                .HasOne(p => p.User)
                .WithMany(u => u.Prestiti)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
