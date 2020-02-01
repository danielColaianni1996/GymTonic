using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GymTonic.Models
{
    public class SchedePersonaliViewModel
    {
        public int SchedaUtenteId { get; set; }
        public int  UtenteId { get; set; }
        public int SchedaId { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInizio { get; set; }
        [Required(ErrorMessage = "Valore obbligatorio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFine { get; set; }
        [Display(Name = "Vuoi rendere la scheda attiva?")]
        public bool Isactive { get; set; }
        internal static List<IndexViewModel> GetIndexViewModel(GymDataContest context)
        {
            var model = new List<IndexViewModel>();
            foreach(var schedaprs in context.SchedePersonali.ToList())
            {
                var temp = new IndexViewModel();
                temp.Id = schedaprs.Id;
                var utente = context.Utenti.Find(schedaprs.UtenteId);
                var scheda = context.Schede.Find(schedaprs.SchedaId).Nome;
                temp.Utente = string.Format("{0} {1}", utente.Nome, utente.Cognome);
                temp.Scheda = scheda;
                temp.IsActive = schedaprs.IsAttiva;
                temp.DataInizio = schedaprs.DataInizio;
                temp.DataFine = schedaprs.DataFine;
                model.Add(temp);
            }
            return model;
        }
        public async static Task<IndexViewModel> GetIndexViewModel(int id,GymDataContest context)
        {
            var model = new IndexViewModel();
            var schedePersonali = await context.SchedePersonali.FindAsync(id);
            model.Id = id;
            model.DataFine = schedePersonali.DataFine;
            model.DataInizio = schedePersonali.DataInizio;
            var utente = context.Utenti.Find(schedePersonali.UtenteId);
            model.Utente = string.Format("{0} {1}", utente.Nome, utente.Cognome);
            var scheda = context.Schede.Find(schedePersonali.SchedaId).Nome;
            model.Scheda = scheda;
            model.IsActive = schedePersonali.IsAttiva;
            //model.Utenti = context.Utenti.ToList();
            //model.Schede = context.Schede.ToList();
            model.DataInizio = schedePersonali.DataInizio;
            model.DataFine = schedePersonali.DataFine;
            return model;
        }
        public static List<IndexViewModel>  ToViewModel( List<SchedePersonali> schede, GymDataContest context)
        {
            var schedeViewModel = new List<IndexViewModel>();
            foreach(var scheda in schede)
            {
                schedeViewModel.Add(new IndexViewModel
                {
                    Id= scheda.Id,
                    Scheda = context.Schede.Where(x => x.Id == scheda.SchedaId).FirstOrDefault().Nome,
                    Utente = context.Utenti.Where(x => x.Id == scheda.UtenteId).FirstOrDefault().Nome,
                    DataInizio = scheda.DataInizio,
                    DataFine =scheda.DataFine
                });
            }
            return schedeViewModel;
        }
        public SchedePersonali toModel(GymDataContest context)
        {
            var result= new SchedePersonali();
            result.Id = 0;
            result.SchedaId = SchedaId;
            result.UtenteId = UtenteId;
            result.IsAttiva = Isactive;
            result.DataInizio = DataInizio;
            result.DataFine = DataFine;
            if(Isactive)
            {
                var schedaPers = context.SchedePersonali.Where(x => x.IsAttiva == true && x.UtenteId == UtenteId && SchedaId == x.SchedaId).FirstOrDefault();
                if(schedaPers!= null)
                {
                    schedaPers.IsAttiva = false;
                    context.Update(schedaPers);
                    context.SaveChanges();
                }
            }
            return result;
        }
    }
    public class IndexViewModel
    {
        public int Id { get; set; }
        public string Utente { get; set; }
        public string Scheda { get; set; }
        [Display(Name = "Vuoi rendere la scheda attiva?")]
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataInizio { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataFine { get; set; }

        public async void Update( GymDataContest context)
        {
            var scheda = await context.SchedePersonali.FindAsync(Id);
            scheda.DataFine = DataFine;
            scheda.DataInizio = DataInizio;
            scheda.IsAttiva = IsActive;
            if (IsActive)
            {
                var schedaPers = context.SchedePersonali.Where(x => x.IsAttiva == true && x.UtenteId == scheda.UtenteId && scheda.SchedaId == x.SchedaId).FirstOrDefault();
                if (schedaPers != null)
                {
                    schedaPers.IsAttiva = false;
                    context.Update(schedaPers);
                    context.SaveChanges();
                }
            }
            context.Update(scheda);
            await context.SaveChangesAsync();
        }
    }
}
