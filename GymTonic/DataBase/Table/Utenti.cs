using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Utenti
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [EmailAddress(ErrorMessage = "Inserisci una mail valida")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Cognome { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNascita { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInserimento { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Sesso { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [Display(Name = "Tipo di orario lavorativo")]
        public int? IdOrari {get; set;}
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Residenza { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public int Eta { get; set; }
    }
}
