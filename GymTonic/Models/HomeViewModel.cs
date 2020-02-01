using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.Models
{
    public class HomeViewModel
    {
        public int Scadenze { get; set;  }
        public int Esercizi { get; set;  }
        public int Utenti { get; set;  }
        public int Schede { get; set;  }
        public List<IndexViewModel> ListaScadenze { get; set; }
        public List<Utenti> ListaUtenti { get; set; }
        public static HomeViewModel GetViewModel (GymDataContest context)
        {
            var model = new HomeViewModel();
            var scadenze = context.SchedePersonali.Where(s => s.DataFine > DateTime.Now.AddDays(-7)).ToList();
            
            model.ListaScadenze = SchedePersonaliViewModel.ToViewModel( scadenze,context);
            model.ListaUtenti  = context.Utenti.OrderByDescending(x=> x.Id).Take(10).ToList();

            model.Scadenze = scadenze.Count();
            model.Esercizi = context.Esercizi.Count();
            model.Utenti = context.Utenti.Count();
            model.Schede = context.SchedeEsercizi.Count();

            return model;
        }

    }
}
