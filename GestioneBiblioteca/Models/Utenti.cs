

using System.ComponentModel.DataAnnotations;

namespace GestioneBiblioteca.Models
{
    public class Utenti
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome utente è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome utente non può superare i 50 caratteri")]
        public string NomeUtente { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato email non valido")]
        [StringLength(100, ErrorMessage = "L'email non può superare i 100 caratteri")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il numero tessera è obbligatorio")]
        [StringLength(20, ErrorMessage = "Il numero tessera non può superare i 20 caratteri")]
        public string NumeroTessera { get; set; }

        [Display(Name = "Utente Sospeso")]
        public bool Sospeso { get; set; } = false;

        // Navigation property per i prestiti (se aggiungerai il model Prestiti)
        public virtual ICollection<Prestito>? Prestiti { get; set; }
    }
}
