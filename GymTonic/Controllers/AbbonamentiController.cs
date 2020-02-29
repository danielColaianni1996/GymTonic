using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymTonic.DataBase;
using GymTonic.DataBase.Table;
using static GymTonic.Models.AbbonamentiViewModel;
using GymTonic.Services;

namespace GymTonic.Controllers
{
    public class AbbonamentiController : Controller
    {
        private readonly GymDataContest _context;

        public AbbonamentiController(GymDataContest context)
        {
            _context = context;
        }

        // GET: Abbonamentis
        public async Task<IActionResult> Index()
        {
            List<IndexViewModel> model = IndexViewModel.ToViewModel(await _context.Abbonamenti.ToListAsync(), _context);
            return View(model);
        }

        // GET: Abbonamentis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abbonamenti = await _context.Abbonamenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abbonamenti == null)
            {
                return NotFound();
            }

            return View(abbonamenti);
        }

        // GET: Abbonamentis/Create
        public IActionResult Create()
        {
            SetCommonViewData();
            return View();
        }

        // POST: Abbonamentis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Abbonamenti abbonamenti)
        {
            if (ModelState.IsValid)
            {
                if (abbonamenti.IsActive == true)
                {
                    var vecchio = GetVecchioAbbonamento(abbonamenti.UtenteId);
                    if(vecchio != null )
                        _context.Update(vecchio);
                }
                _context.Add(abbonamenti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(abbonamenti);
        }

        // GET: Abbonamentis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abbonamenti = await _context.Abbonamenti.FindAsync(id);
            if (abbonamenti == null)
            {
                return NotFound();
            }
            SetCommonViewData();
            return View(abbonamenti);
        }

        // POST: Abbonamentis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Abbonamenti abbonamenti)
        {
            if (id != abbonamenti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (abbonamenti.IsActive == true)
                    {
                        var vecchio = GetVecchioAbbonamento(abbonamenti.UtenteId);
                        if (vecchio != null)
                            _context.Update(vecchio);
                    }
                    _context.Update(abbonamenti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbbonamentiExists(abbonamenti.Id))
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
            SetCommonViewData();
            return View(abbonamenti);
        }

        // GET: Abbonamentis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abbonamenti = IndexViewModel.ToViewModel(await _context.Abbonamenti
                .FirstOrDefaultAsync(m => m.Id == id), _context);
            if (abbonamenti == null)
            {
                return NotFound();
            }

            return View(abbonamenti);
        }
        public async Task<ActionResult> SendPromemoria(int Id)
        {
            var abbonamento = await _context.Abbonamenti.FindAsync(Id);
            var mailTo = _context.Utenti.Find(abbonamento.UtenteId).Mail;
            MailServices mail = new MailServices();
            if (mail.SendPromemoria(mailTo, abbonamento.FineAbbonamento.ToShortDateString()))
                return Json(new { status = "success" });
            else
                return Json(new { status = "error" });
        }
        // POST: Abbonamentis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abbonamenti = await _context.Abbonamenti.FindAsync(id);
            _context.Abbonamenti.Remove(abbonamenti);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbbonamentiExists(int id)
        {
            return _context.Abbonamenti.Any(e => e.Id == id);
        }
        private void SetCommonViewData()
        {
            ViewData["listaUtenti"] = GetUtenti();
            ViewData["listaAbbonamenti"] = GetTipiAbbomaneti();
        }
        private List<SelectListItem> GetUtenti()
        {
            var users = _context.Utenti;
            List<SelectListItem> result = new List<SelectListItem> {
                new SelectListItem
                {
                    Text= "Seleziona un utente",
                    Value = ""
                }
            };

            foreach (var user in users)
            {
                var temp = new SelectListItem();
                temp.Text = user.Nome + " " + user.Cognome;
                temp.Value = user.Id.ToString();
                result.Add(temp);
            }

            return result;
        }
        private List<SelectListItem> GetTipiAbbomaneti()
        {
            var abbonamenti = _context.TipiAbbonamenti;
            List<SelectListItem> result = new List<SelectListItem>{
                new SelectListItem
                {
                    Text= "Seleziona un tipo di abbonamento",
                    Value = ""
                }
            };

            foreach (var abbonamento in abbonamenti)
            {
                var temp = new SelectListItem();
                temp.Text = abbonamento.Descrizione;
                temp.Value = abbonamento.Id.ToString();
                result.Add(temp);
            }

            return result;
        }
        private Abbonamenti GetVecchioAbbonamento(int userId)
        {
            var vecchiAbbonamenti = _context.Abbonamenti.Where(a => a.IsActive == true && a.UtenteId == userId).FirstOrDefault();
            if (vecchiAbbonamenti != null)
                vecchiAbbonamenti.IsActive = false;
            
            return vecchiAbbonamenti;
        }
    }
}
