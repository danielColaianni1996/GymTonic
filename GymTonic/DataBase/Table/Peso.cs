using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Peso
    {
        public int Id { get; set; }
        public int UtenteId { get; set; }
        public double MassaGrassa { get; set; }
        public double MassaMagra { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInserimento { get; set; }
    
    }
}
