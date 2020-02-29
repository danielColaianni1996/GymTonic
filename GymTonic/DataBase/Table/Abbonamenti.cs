using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Abbonamenti
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [Display(Name ="Utente")]
        public int UtenteId { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [Display(Name = "Tipo abbonamento")]
        public int TipoAbbonamentoId { get; set; }
        [Display(Name = "Abbonamento Rinnovato?")]
        public bool IsRinnovo {get; set;}
        [Required(ErrorMessage = "Valore obbligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Inizio")]
        public DateTime InizioAbbonamento { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fine")]
        public DateTime FineAbbonamento { get; set; }
        [Display(Name = "Abbonamento Attivo?")]
        public bool IsActive { get; set; }
    }
}
