using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.DataBase.Table
{
    public class SchedePersonali
    {
        public int Id { get; set; }
        public DateTime DataFine { get; set; }
        public DateTime DataInizio { get; set; }
        public int UtenteId { get; set; }
        public int SchedaId { get; set; }
        public bool IsAttiva { get; set; }
    }
}
