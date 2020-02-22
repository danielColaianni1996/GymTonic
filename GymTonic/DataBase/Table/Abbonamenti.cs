using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class Abbonamenti
    {
        public int Id { get; set; }
        public int UtenteId { get; set; }
        public int TipoAbbonamentoId { get; set; }
        public int CounterAbbonamenti {get; set;}
        public DateTime InizioAbbonamento { get; set; }
        public DateTime FineAbbonamento { get; set; }
    }
}
