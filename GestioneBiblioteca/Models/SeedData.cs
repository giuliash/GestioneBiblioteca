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
                    Titolo = "Se questo è un uomo",
                    Autore = "Primo Levi",
                    AnnoDiPubblicazione = DateTime.Parse("1947-10-11"),
                    Genere = "Memorialistica"
                },
                new Libro
                {
                    Titolo = "Il Gattopardo",
                    Autore = "Giuseppe Tomasi di Lampedusa",
                    AnnoDiPubblicazione = DateTime.Parse("1958-11-1"),
                    Genere = "Narrativa"
                },
                new Libro
                {
                    Titolo = "Cent'anni di solitudine",
                    Autore = "Gabriel García Márquez",
                    AnnoDiPubblicazione = DateTime.Parse("1967-6-5"),
                    Genere = "Realismo magico"
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
                    Titolo = "Orgoglio e Pregiudizio",
                    Autore = "Jane Austen",
                    AnnoDiPubblicazione = DateTime.Parse("1813-1-28"),
                    Genere = "Romantico"
                },
                new Libro
                {
                    Titolo = "Il Piccolo Principe",
                    Autore = "Antoine de Saint-Exupéry",
                    AnnoDiPubblicazione = DateTime.Parse("1943-4-6"),
                    Genere = "Favola"
                },
                new Libro
                {
                    Titolo = "Harry Potter e la Pietra Filosofale",
                    Autore = "J.K. Rowling",
                    AnnoDiPubblicazione = DateTime.Parse("1997-6-26"),
                    Genere = "Fantasy"
                },
                new Libro
                {
                    Titolo = "Il Signore degli Anelli - La Compagnia dell'Anello",
                    Autore = "J.R.R. Tolkien",
                    AnnoDiPubblicazione = DateTime.Parse("1954-7-29"),
                    Genere = "Fantasy"
                },
                new Libro
                {
                    Titolo = "Don Chisciotte della Mancia",
                    Autore = "Miguel de Cervantes",
                    AnnoDiPubblicazione = DateTime.Parse("1605-1-16"),
                    Genere = "Classico"
                },
                new Libro
                {
                    Titolo = "La Divina Commedia",
                    Autore = "Dante Alighieri",
                    AnnoDiPubblicazione = DateTime.Parse("1320-1-1"),
                    Genere = "Poesia epica"
                },
                new Libro
                {
                    Titolo = "I Promessi Sposi",
                    Autore = "Alessandro Manzoni",
                    AnnoDiPubblicazione = DateTime.Parse("1827-4-15"),
                    Genere = "Storico"
                },
                new Libro
                {
                    Titolo = "Il Grande Gatsby",
                    Autore = "F. Scott Fitzgerald",
                    AnnoDiPubblicazione = DateTime.Parse("1925-4-10"),
                    Genere = "Narrativa americana"
                },
                new Libro
                {
                    Titolo = "Moby Dick",
                    Autore = "Herman Melville",
                    AnnoDiPubblicazione = DateTime.Parse("1851-10-18"),
                    Genere = "Avventura"
                },
                new Libro
                {
                    Titolo = "Guerra e Pace",
                    Autore = "Lev Tolstoj",
                    AnnoDiPubblicazione = DateTime.Parse("1869-1-1"),
                    Genere = "Storico"
                },
                new Libro
                {
                    Titolo = "Il Processo",
                    Autore = "Franz Kafka",
                    AnnoDiPubblicazione = DateTime.Parse("1925-4-26"),
                    Genere = "Kafkiano"
                },
                new Libro
                {
                    Titolo = "Cime Tempestose",
                    Autore = "Emily Brontë",
                    AnnoDiPubblicazione = DateTime.Parse("1847-12-1"),
                    Genere = "Gotico"
                },
                new Libro
                {
                    Titolo = "Il Conte di Montecristo",
                    Autore = "Alexandre Dumas",
                    AnnoDiPubblicazione = DateTime.Parse("1844-8-28"),
                    Genere = "Avventura"
                },
                new Libro
                {
                    Titolo = "Fahrenheit 451",
                    Autore = "Ray Bradbury",
                    AnnoDiPubblicazione = DateTime.Parse("1953-10-19"),
                    Genere = "Fantascienza"
                },
                new Libro
                {
                    Titolo = "Il Ritratto di Dorian Gray",
                    Autore = "Oscar Wilde",
                    AnnoDiPubblicazione = DateTime.Parse("1890-6-20"),
                    Genere = "Gotico"
                },
                new Libro
                {
                    Titolo = "Lo Straniero",
                    Autore = "Albert Camus",
                    AnnoDiPubblicazione = DateTime.Parse("1942-5-19"),
                    Genere = "Esistenzialista"
                },
                new Libro
                {
                    Titolo = "Anna Karenina",
                    Autore = "Lev Tolstoj",
                    AnnoDiPubblicazione = DateTime.Parse("1877-4-1"),
                    Genere = "Realista"
                },
                new Libro
                {
                    Titolo = "Il Vecchio e il Mare",
                    Autore = "Ernest Hemingway",
                    AnnoDiPubblicazione = DateTime.Parse("1952-9-1"),
                    Genere = "Narrativa"
                },
                new Libro
                {
                    Titolo = "Delitto e Castigo",
                    Autore = "Fëdor Dostoevskij",
                    AnnoDiPubblicazione = DateTime.Parse("1866-1-1"),
                    Genere = "Psicologico"
                },
                new Libro
                {
                    Titolo = "Le Avventure di Tom Sawyer",
                    Autore = "Mark Twain",
                    AnnoDiPubblicazione = DateTime.Parse("1876-6-1"),
                    Genere = "Avventura"
                },
                new Libro
                {
                    Titolo = "Dune",
                    Autore = "Frank Herbert",
                    AnnoDiPubblicazione = DateTime.Parse("1965-8-1"),
                    Genere = "Fantascienza"
                },
                new Libro
                {
                    Titolo = "Il Mondo Nuovo",
                    Autore = "Aldous Huxley",
                    AnnoDiPubblicazione = DateTime.Parse("1932-8-30"),
                    Genere = "Distopico"
                },
                new Libro
                {
                    Titolo = "Robinson Crusoe",
                    Autore = "Daniel Defoe",
                    AnnoDiPubblicazione = DateTime.Parse("1719-4-25"),
                    Genere = "Avventura"
                },
                new Libro
                {
                    Titolo = "Il Castello",
                    Autore = "Franz Kafka",
                    AnnoDiPubblicazione = DateTime.Parse("1926-4-1"),
                    Genere = "Kafkiano"
                },
                new Libro
                {
                    Titolo = "Jane Eyre",
                    Autore = "Charlotte Brontë",
                    AnnoDiPubblicazione = DateTime.Parse("1847-10-16"),
                    Genere = "Gotico"
                }
            );
            context.SaveChanges();
        }
    }
}

