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

namespace GymTonic.Controllers
{
    public class SchedeEserciziController : Controller
    {
        private readonly GymDataContest _context;

        public SchedeEserciziController(GymDataContest context)
        {
            _context = context;
        }

        // GET: SchedeEsercizi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Schede.ToListAsync());
        }

        // GET: SchedeEsercizi/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedeEsercizi = SchedeViewModel.GetViewModel(id??0, _context);
            if (schedeEsercizi == null)
            {
                return NotFound();
            }

            return View(schedeEsercizi);
        }

        // GET: SchedeEsercizi/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetListEsercizi ()
        {
            List<SelectListItem> esercizi = new List<SelectListItem>();
            foreach( var esercizio in _context.Esercizi.ToList())
            {
                esercizi.Add(new SelectListItem
                {
                    Text = esercizio.NomeEsercizio,
                    Value = esercizio.ID.ToString()

                });
            }
            return Json(esercizi);
        }
        // POST: SchedeEsercizi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schede scheda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheda);
                await _context.SaveChangesAsync();
                var idScheda = _context.Schede.ToList().Last();
                var id = SchedeViewModel.CreationSchedeViewModel(idScheda.Id, _context);
                return RedirectToAction("Edit", new { id = idScheda.Id });
            }
            return RedirectToAction("Create");
        }
        public async Task<ActionResult> EliminaEsercizio(int idScheda, int idEsercizio)
        {
             var scheda = _context.SchedeEsercizi.Where(sc => sc.IdScheda == idScheda && sc.IdEsercizio == idEsercizio).FirstOrDefault();
            if (scheda != null)
            {
                _context.Remove(scheda);
                await _context.SaveChangesAsync();
            }
            return Json("true");
        }
        // GET: SchedeEsercizi/Edit/5
        public IActionResult Edit(int id)
        {
            
            var schedeEsercizi = _context.SchedeEsercizi.Where(x=> x.IdScheda==id);
            if (schedeEsercizi == null)
            {
                return NotFound();
            }
            var model = SchedeViewModel.GetViewModel(id, _context);
            ViewData["ListaEserciziRiscaldamento"] = getEserciziRiscaldamento();
            ViewData["ListaEsercizi"] = getEsercizi();
            return View(model);
        }

        // POST: SchedeEsercizi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SchedeViewModel schedeEsercizi)
        {
            SchedeViewModel.UpdateModel(schedeEsercizi,_context);
            ViewData["ListaEserciziRiscaldamento"] = getEserciziRiscaldamento();
            ViewData["ListaEsercizi"] = getEsercizi();
            return RedirectToAction(nameof(Index));
        }

        // GET: SchedeEsercizi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var scheda = await _context.Schede
                .FirstOrDefaultAsync(m => m.Id == id);

            if (scheda == null)
            {
                return NotFound();
            }

            return View(scheda);
        }

        // POST: SchedeEsercizi/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var scheda = await _context.Schede.FindAsync(id);
            SchedeViewModel.DeleteSchedaAsync(scheda, _context);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult AddEsercizio(bool IsRiscaldamento)
        {
            
            ViewData["ListaEserciziRiscaldamento"] = getEserciziRiscaldamento();
            ViewData["ListaEsercizi"] = getEsercizi();
            EsercizioViewModel es = new EsercizioViewModel();
            if (IsRiscaldamento)
                es.IsRiscaldamento = true;
            es.Ripetizioni = 0;
            return PartialView("_eserciziPartial", es);
        }
        private bool SchedeEserciziExists(int id)
        {
            return _context.SchedeEsercizi.Any(e => e.Id == id);
        }
        private List<SelectListItem> getEserciziRiscaldamento()
        {
            List<SelectListItem> esercizi = new List<SelectListItem>();
            esercizi.Add(new SelectListItem
            {
                Text = "seleziona un esercizio",
                Value = "0"

            });

            foreach (var esercizio in _context.Esercizi.Where(e => e.IsRiscaldamento == true).ToList())
            {
                esercizi.Add(new SelectListItem
                {
                    Text = esercizio.NomeEsercizio,
                    Value = esercizio.ID.ToString()

                });
            }
            return esercizi;
        }
        private List<SelectListItem> getEsercizi()
        {
            List<SelectListItem> esercizi = new List<SelectListItem>();
            esercizi.Add(new SelectListItem
            {
                Text = "seleziona un esercizio",
                Value = "0"

            });

            foreach (var esercizio in _context.Esercizi.Where(e => (e.IsRiscaldamento) == false).ToList())
            {
                esercizi.Add(new SelectListItem
                {
                    Text = esercizio.NomeEsercizio,
                    Value = esercizio.ID.ToString()

                });
            }
            return esercizi;
        }
    }
}
