using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.Models
{
    public class AbbonamentiViewModel
    {
        public class IndexViewModel
        {
            public int Id { get; set; }
            public string Utente { get; set; }
            public string Abbonamenti { get; set; }
            [Display(Name = "Rinnovo?")]
            public bool Rinnovo { get; set; }
            [Display(Name = "Vuoi renderlo attivo?")]
            public bool Attivo { get; set; }
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DataInizio { get; set; }
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DataFine { get; set; }

            public static List<IndexViewModel> ToViewModel(List<Abbonamenti> abbonamenti, GymDataContest context)
            {
                List<IndexViewModel> model = new List<IndexViewModel>();
                foreach (var abbonamento in abbonamenti)
                {
                    model.Add(ToViewModel(abbonamento, context));
                }
                return model;
            }
            public static IndexViewModel ToViewModel(Abbonamenti abbonamento, GymDataContest context)
            {
                var utente = context.Utenti.Where(x => x.Id == abbonamento.UtenteId).FirstOrDefault();
                IndexViewModel model = new IndexViewModel
                {
                    Id = abbonamento.Id,
                    Abbonamenti = context.TipiAbbonamenti.Where(x => x.Id == abbonamento.TipoAbbonamentoId).FirstOrDefault().Descrizione,
                    Utente = utente.Nome + " " + utente.Cognome,
                    Rinnovo = abbonamento.IsRinnovo,
                    DataInizio = abbonamento.InizioAbbonamento,
                    DataFine = abbonamento.FineAbbonamento,
                    Attivo = abbonamento.IsActive
                };
                return model;
            }
        }
    }
}
