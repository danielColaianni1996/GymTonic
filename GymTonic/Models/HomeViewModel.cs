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
        public int Scadenze { get; set; }
        public int Esercizi { get; set; }
        public int Utenti { get; set; }
        public int Schede { get; set; }
        public int Abbonamenti { get; set; }
        public List<IndexViewModel> ListaScadenze { get; set; }
        public List<AbbonamentiViewModel.IndexViewModel> ListaAbbonamentiScadenze { get; set; }
        public List<Utenti> ListaUtenti { get; set; }
        public List<int> UtentiChart { get; set; } = new List<int>();
        public AbbonamentiChart AbbonamentiChart { get; set; } = new AbbonamentiChart();
        public static HomeViewModel GetViewModel(GymDataContest context)
        {
            var model = new HomeViewModel();
            var scadenze = context.SchedePersonali.Where(s => s.DataFine < DateTime.Now.AddDays(7) && s.DataFine >= DateTime.Now).ToList();
            var abbonamentiScadenza = context.Abbonamenti.Where(a => a.FineAbbonamento < DateTime.Now.AddDays(7) && a.FineAbbonamento >= DateTime.Now).ToList();

            model.ListaScadenze = SchedePersonaliViewModel.ToViewModel(scadenze, context);
            model.ListaAbbonamentiScadenze = AbbonamentiViewModel.IndexViewModel.ToViewModel(abbonamentiScadenza, context);
            model.ListaUtenti = context.Utenti.OrderByDescending(x => x.Id).Take(10).ToList();

            model.Scadenze = scadenze.Count();
            model.Esercizi = context.Esercizi.Count();
            model.Utenti = context.Utenti.Count();
            model.Schede = context.Schede.Count();

            model.Abbonamenti = context.Abbonamenti.Where(a => a.IsActive ).Count();
            for (int i = 1; i <= 12; i++)
            {
                model.UtentiChart.Add(context.Utenti.Where(x => x.DataInserimento.Month == i).Count());
                var abbonamenti = context.Abbonamenti.Where(x => x.InizioAbbonamento.Month == i && x.IsActive == true).ToList();
                int nuovi = 0;
                int rinnovati = 0;
                int persi = 0;
                foreach(var abb in abbonamenti)
                {
                    var isRinnovo = context.Abbonamenti.Where(x => x.UtenteId == abb.UtenteId).ToList().Count;
                    if (isRinnovo > 1)
                        rinnovati++;
                    else
                        nuovi++;
                }
                
                var abbonamentiPersi = context.Abbonamenti.Where(x => x.FineAbbonamento.Month == i && x.IsActive && x.FineAbbonamento.Month < DateTime.Now.Month).ToList();
                foreach (var abb in abbonamentiPersi)
                {
                    var isperso = context.Abbonamenti.Where(x => x.UtenteId == abb.UtenteId && x.InizioAbbonamento > abb.FineAbbonamento).ToList().Count;
                    if (!(isperso> 0))
                        persi++;
                }

                model.AbbonamentiChart.Nuovi.Add(nuovi);
                model.AbbonamentiChart.Rinnovati.Add(rinnovati);
                model.AbbonamentiChart.Persi.Add(persi);
            }

           

            return model;
        }

    }
    public class AbbonamentiChart
    {
        public List<int> Nuovi { get; set; } = new List<int>();
        public List<int> Persi { get; set; } = new List<int>();
        public List<int> Rinnovati { get; set; } = new List<int>();
    }
}
