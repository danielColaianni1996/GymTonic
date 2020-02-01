using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Schede
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        //[Display(Name = "Inserisci una breve descrizione sulla scheda")]
        public string DescrizioneScheda { get; set; }
        [Required]
        public string Nome { get; set; }

    }
}
