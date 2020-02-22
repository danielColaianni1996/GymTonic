using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymTonic.DataBase;
using GymTonic.DataBase.Table;

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
            return View(await _context.Abbonamenti.ToListAsync());
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
            return View();
        }

        // POST: Abbonamentis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UtenteId,TipoAbbonamentoId,CounterAbbonamenti,InizioAbbonamento,FineAbbonamento")] Abbonamenti abbonamenti)
        {
            if (ModelState.IsValid)
            {
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
            return View(abbonamenti);
        }

        // POST: Abbonamentis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UtenteId,TipoAbbonamentoId,CounterAbbonamenti,InizioAbbonamento,FineAbbonamento")] Abbonamenti abbonamenti)
        {
            if (id != abbonamenti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            return View(abbonamenti);
        }

        // GET: Abbonamentis/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
    }
}
