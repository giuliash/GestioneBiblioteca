using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestioneBiblioteca.Models
{
    public class Prestito
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La data di inizio è obbligatoria")]
        [Display(Name = "Data Inizio Prestito")]
        public DateTime DataInizio { get; set; }

        [Display(Name = "Data Fine Prestito")]
        public DateTime? DataFine { get; set; }


        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Utenti User { get; set; }

        [Required]
        public int LibroId { get; set; }

        [ForeignKey("LibroId")]
        public virtual Libro Libro { get; set; }


    }
}
