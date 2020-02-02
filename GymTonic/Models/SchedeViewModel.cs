using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.Models
{
    public class SchedeViewModel
    {
        public int Id { get; set; }
        public List<EsercizioViewModel> Esercizi { get; set; }
        public int IdScheda { get; set; }
        public string NomeScheda { get; set; }
        public string DescrizioneScheda { get; set;}
        public static int CreationSchedeViewModel(int id, GymDataContest context)
        {
            Schede scheda = context.Schede.Where(x => x.Id == id).FirstOrDefault();
            var schedaEs = context.SchedeEsercizi.Where(x => x.IdScheda == scheda.Id);
            if (schedaEs.Count()>0)
            {
                context.SchedeEsercizi.RemoveRange(schedaEs);
            }
            SchedeEsercizi newScheda = new SchedeEsercizi()
            {
                IdEsercizio = 0,
                IdScheda = scheda.Id
            };
            context.SchedeEsercizi.Add(newScheda);
            context.SaveChanges();
            var result = context.SchedeEsercizi.ToList().Last().Id;
            return result;
        }
        public static SchedeViewModel GetViewModel (int id, GymDataContest context)
        {
            List<SchedeEsercizi> schede= context.SchedeEsercizi.Where(x => x.IdScheda == id).ToList();
            //genero gli esercizi 
            SchedeViewModel viewModel = new SchedeViewModel();
            //viewModel.Id = id;
            viewModel.IdScheda = id;
            viewModel.NomeScheda= context.Schede.Where(s => s.Id == id).FirstOrDefault()?.Nome;
            viewModel.DescrizioneScheda =context.Schede.Where(s=> s.Id==id).FirstOrDefault()?.DescrizioneScheda;
            List<EsercizioViewModel> esercizi = new List<EsercizioViewModel>();
            foreach(var scheda in schede)
            {
                var esercizio = context.Esercizi.Where(x => x.ID == scheda.IdEsercizio).FirstOrDefault();
                if(esercizio!=null)
                    esercizi.Add(new EsercizioViewModel() { 
                        ID=esercizio.ID,
                        Descrizione=esercizio.Descrizione,
                        IsRiscaldamento= esercizio.IsRiscaldamento,
                        NomeEsercizio=esercizio.NomeEsercizio,
                        Ripetizioni= scheda.Ripetizioni
                    });
            }
            viewModel.Esercizi = esercizi;
            return viewModel;
        }

        public static void UpdateModel(SchedeViewModel schedeEsercizi, GymDataContest context)
        {
            int schedaId = schedeEsercizi.IdScheda;
            context.SchedeEsercizi.RemoveRange(context.SchedeEsercizi.Where(x => x.IdScheda == schedaId).ToList());
            foreach (var esercizio in schedeEsercizi.Esercizi)
            {
                if (esercizio.ID != 0)
                {
                    context.Add(new SchedeEsercizi()
                    {
                        IdEsercizio = esercizio.ID,
                        IdScheda = schedaId,
                        Ripetizioni = esercizio.Ripetizioni
                    });
                }
            }
            var scheda = context.Schede.Find(schedaId);
            scheda.DescrizioneScheda = schedeEsercizi.DescrizioneScheda;
            scheda.Nome = schedeEsercizi.NomeScheda;
            context.Update(scheda);
            context.SaveChanges();
        }

        public async static void DeleteSchedaAsync(Schede scheda, GymDataContest context)
        {
            context.SchedeEsercizi.RemoveRange(context.SchedeEsercizi.Where(x => x.IdScheda == scheda.Id).ToList());
            context.SchedePersonali.RemoveRange(context.SchedePersonali.Where(x => x.SchedaId == scheda.Id).ToList());
            context.Remove(scheda);
            await context.SaveChangesAsync();
        }
    }
}
