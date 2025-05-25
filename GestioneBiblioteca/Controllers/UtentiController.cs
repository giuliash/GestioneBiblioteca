using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestioneBiblioteca.Data;
using GestioneBiblioteca.Models;

namespace GestioneBiblioteca.Controllers
{
    public class UtentiController : Controller
    {
        private readonly GestioneBibliotecaContext _context;

        public UtentiController(GestioneBibliotecaContext context)
        {
            _context = context;
        }

        // GET: Utenti
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utenti.ToListAsync());
        }

        // GET: Utenti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utenti == null)
            {
                return NotFound();
            }

            return View(utenti);
        }

        // GET: Utenti/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utenti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeUtente,Email,NumeroTessera,Sospeso")] Utenti utenti)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utenti);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utenti);
        }

        // GET: Utenti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utenti.FindAsync(id);
            if (utenti == null)
            {
                return NotFound();
            }
            return View(utenti);
        }

        // POST: Utenti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeUtente,Email,NumeroTessera,Sospeso")] Utenti utenti)
        {
            if (id != utenti.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utenti);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtentiExists(utenti.Id))
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
            return View(utenti);
        }

        // GET: Utenti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utenti = await _context.Utenti
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utenti == null)
            {
                return NotFound();
            }

            return View(utenti);
        }

        // POST: Utenti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utenti = await _context.Utenti.FindAsync(id);
            if (utenti != null)
            {
                _context.Utenti.Remove(utenti);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtentiExists(int id)
        {
            return _context.Utenti.Any(e => e.Id == id);
        }
    }
}
