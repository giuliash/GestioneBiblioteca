using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestioneBiblioteca.Data;
using System;
using System.Linq;

namespace GestioneBiblioteca.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new GestioneBibliotecaContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<GestioneBibliotecaContext>>()))
        {
            // Look for any movies.
            if (context.Libro.Any())
            {
                return;   // DB has been seeded
            }
            context.Libro.AddRange(
                new Libro
                {
                    Titolo = "Il nome della Rosa",
                    Autore = "Umberto Eco",
                    AnnoDiPubblicazione = DateTime.Parse("1980-10-1"),
                    Genere = "Storico"
                },
                new Libro
                {
                    Titolo = "1984",
                    Autore = "George Orwell",
                    AnnoDiPubblicazione = DateTime.Parse("1949-6-8"),
                    Genere = "Distopico"
                },
                new Libro
                {
                    Titolo = "Il piccolo principe",
                    Autore = "Antoine de Saint-Exupéry",
                    AnnoDiPubblicazione = DateTime.Parse("1943-4-6"),
                    Genere = "Fiaba"
                },
                new Libro
                {
                    Titolo = "Harry Potter e la pietra filosofale",
                    Autore = "J.K. Rowling",
                    AnnoDiPubblicazione = DateTime.Parse("1997-6-26"),
                    Genere = "Fantasy"
                }
            );
            context.SaveChanges();
        }
    }
}

