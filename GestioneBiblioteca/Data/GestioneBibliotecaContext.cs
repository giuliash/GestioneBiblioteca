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
        public GestioneBibliotecaContext (DbContextOptions<GestioneBibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<GestioneBiblioteca.Models.Libro> Libro { get; set; } = default!;
    }
}
