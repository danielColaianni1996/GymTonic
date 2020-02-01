using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class SchedeEsercizi
    {
        public int Id { get; set; }
        public int IdScheda { get; set; }
        public int IdEsercizio { get; set; }
        public int Ripetizioni { get; set; }
    }
}
