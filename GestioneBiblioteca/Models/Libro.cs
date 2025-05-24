using System.ComponentModel.DataAnnotations;

namespace GestioneBiblioteca.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string? Titolo { get; set; }
        public string? Autore { get; set; }

        [DataType(DataType.Date)]
        public DateTime AnnoDiPubblicazione { get; set; }
        public string? Genere { get; set; }
    }
}
