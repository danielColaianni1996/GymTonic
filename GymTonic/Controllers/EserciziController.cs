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
    public class EserciziController : Controller
    {
        private readonly GymDataContest _context;

        public EserciziController(GymDataContest context)
        {
            _context = context;
        }

        // GET: Esercizi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Esercizi.ToListAsync());
        }

        // GET: Esercizi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esercizi = await _context.Esercizi
                .FirstOrDefaultAsync(m => m.ID == id);
            if (esercizi == null)
            {
                return NotFound();
            }

            return View(esercizi);
        }

        // GET: Esercizi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Esercizi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Esercizi esercizi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(esercizi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(esercizi);
        }

        // GET: Esercizi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esercizi = await _context.Esercizi.FindAsync(id);
            if (esercizi == null)
            {
                return NotFound();
            }
            return View(esercizi);
        }

        // POST: Esercizi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,NomeEsercizio,Descrizione,Ripetizioni")] Esercizi esercizi)
        {
            if (id != esercizi.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(esercizi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EserciziExists(esercizi.ID))
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
            return View(esercizi);
        }

        // GET: Esercizi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var esercizi = await _context.Esercizi
                .FirstOrDefaultAsync(m => m.ID == id);
            if (esercizi == null)
            {
                return NotFound();
            }

            return View(esercizi);
        }

        // POST: Esercizi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var esercizi = await _context.Esercizi.FindAsync(id);
            _context.Esercizi.Remove(esercizi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EserciziExists(int id)
        {
            return _context.Esercizi.Any(e => e.ID == id);
        }
    }
}
