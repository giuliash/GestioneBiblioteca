using System;
using System.ComponentModel.DataAnnotations;

namespace GestioneBiblioteca.Models
{
    public class Donazione
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Inserisci un'email valida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "L'importo è obbligatorio")]
        [Range(1, 10000, ErrorMessage = "L'importo deve essere tra 1 e 10.000 €")]
        public decimal Importo { get; set; }

        public DateTime DataDonazione { get; set; } = DateTime.Now;

        public string? PaymentIntentId { get; set; }

        public bool PagamentoRiuscito { get; set; } = false;
    }
}