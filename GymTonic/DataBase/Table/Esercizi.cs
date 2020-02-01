using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Esercizi
    {
        public int ID { get; set;  }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string NomeEsercizio { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "Valore obbligatorio")]
        [Display(Name ="Esercizio di riscaldamento?")]
        public bool IsRiscaldamento { get; set; }
    }
}
