using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using GymTonic.Models;
using GymTonic.Services;
using System.Text;

namespace GymTonic.Controllers
{
    public class SchedePersonaliController : Controller
    {
        private readonly GymDataContest _context;

        public SchedePersonaliController(GymDataContest context)
        {
            _context = context;
        }

        // GET: SchedePersonalis
        public IActionResult Index()
        {
            var model = SchedePersonaliViewModel.GetIndexViewModel(_context);
            return View(model);
        }

        // GET: SchedePersonalis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var scheda = await _context.SchedePersonali
                .FirstOrDefaultAsync(m => m.Id == id);

            if (scheda == null)
            {
                return NotFound();
            }
            var schedeEsercizi = SchedeViewModel.GetViewModel(id ?? 0, _context);
            if (schedeEsercizi == null)
            {
                return NotFound();
            }

            return View(schedeEsercizi);
        }

        // GET: SchedePersonalis/Create
        public IActionResult Create()
        {
            //var model = SchedePersonaliViewModel.GetViewModel(_context);
            ViewData["ListaUtenti"] = getUtentiList();
            ViewData["ListaSchede"] = getSchedeList();
            return View();
        }

        private List<SelectListItem> getSchedeList()
        {
            List<SelectListItem> schede = new List<SelectListItem>();
            schede.Add(new SelectListItem
            {
                Text = "seleziona una scheda",
                Value = "0"

            });

            foreach (var scheda in _context.Schede.ToList())
            {
                schede.Add(new SelectListItem
                {
                    Text = scheda.Nome,
                    Value = scheda.Id.ToString()

                });
            }
            return schede;
        }

        private List<SelectListItem> getUtentiList()
        {
            List<SelectListItem> utenti = new List<SelectListItem>();
            utenti.Add(new SelectListItem
            {
                Text = "seleziona un Utente",
                Value = "0"

            });

            foreach (var utente in _context.Utenti.ToList())
            {
                utenti.Add(new SelectListItem
                {
                    Text = string.Format("{0} {1}", utente.Nome, utente.Cognome),
                    Value = utente.Id.ToString()

                });
            }
            return utenti;
        }

        // POST: SchedePersonalis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchedePersonaliViewModel schedePersonali)
        {
            if (ModelState.IsValid)
            {
                if (schedePersonali.UtenteId == 0)
                {
                    ModelState.AddModelError("UtenteId", "selezionare un utente valido");
                }
                else
                {
                    var schedaPers = schedePersonali.toModel(_context);
                    _context.Add(schedaPers);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ListaUtenti"] = getUtentiList();
            ViewData["ListaSchede"] = getSchedeList();
            return View(schedePersonali);
        }

        // GET: SchedePersonalis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IndexViewModel schedePersonali = await SchedePersonaliViewModel.GetIndexViewModel(id ?? 0, _context);
            if (schedePersonali == null)
            {
                return NotFound();
            }
            return View(schedePersonali);
        }

        // POST: SchedePersonalis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IndexViewModel schedePersonali)
        {
            if (schedePersonali.Id != schedePersonali.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (schedePersonali.DataInizio > schedePersonali.DataFine)
                {
                    ModelState.AddModelError("DataInizio", "La data di inizio è maggiore della data di scadenza");
                    return View(schedePersonali);
                }
                try
                {
                    schedePersonali.Update(_context);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchedePersonaliExists(schedePersonali.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(schedePersonali);
        }

        // GET: SchedePersonalis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedePersonali = await SchedePersonaliViewModel.GetIndexViewModel(id ?? 0, _context);
            if (schedePersonali == null)
            {
                return NotFound();
            }

            return View(schedePersonali);
        }

        // POST: SchedePersonalis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedePersonali = await _context.SchedePersonali.FindAsync(id);
            _context.SchedePersonali.Remove(schedePersonali);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchedePersonaliExists(int id)
        {
            return _context.SchedePersonali.Any(e => e.Id == id);
        }
        public ActionResult SendScheda(int id)
        {
            var schedaUtente = _context.SchedePersonali.Find(id);
            var scheda = _context.SchedeEsercizi.Where(x => x.IdScheda == schedaUtente.SchedaId).ToList();
            var mailUser = _context.Utenti.Find(schedaUtente.UtenteId).Mail;
            Dictionary<string, string> dataTemplate = new Dictionary<string, string>();
            StringBuilder builderRiscaldamento = new StringBuilder();
            StringBuilder builderWod = new StringBuilder();
            foreach (var esercizio in scheda)
            {
                var ese = _context.Esercizi.Find(esercizio.IdEsercizio);
                if (ese.IsRiscaldamento)
                {
                    builderRiscaldamento.Append(string.Format("<tr> <td> {0}</td> <td>{1}</td> <td> {2}</td> </tr>", ese.NomeEsercizio, esercizio.Ripetizioni, ese.Descrizione));
                }
                else
                {
                    builderWod.Append(string.Format("<tr> <td> {0}</td> <td>{1}</td> <td> {2}</td> </tr>", ese.NomeEsercizio, esercizio.Ripetizioni, ese.Descrizione));
                }
            }
            dataTemplate.Add("%TableBody%", builderRiscaldamento.ToString());
            dataTemplate.Add("%TableBodyWod%", builderWod.ToString());
            MailServices mail = new MailServices();
            mail.LoadTemplate();
            mail.CompilaTemplate(dataTemplate);
            var result = mail.SendSchedaMail(mailUser);
            if (result)
                return Json(new { status = "Success" });
            else
                return Json(new { status = "Error" });
        }
    }
}
