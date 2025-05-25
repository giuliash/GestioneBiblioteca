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
    public class PrestitiController : Controller
    {
        private readonly GestioneBibliotecaContext _context;

        public PrestitiController(GestioneBibliotecaContext context)
        {
            _context = context;
        }

        // GET: Prestiti
        public async Task<IActionResult> Index()
        {
            var gestioneBibliotecaContext = _context.Prestiti.Include(p => p.Libro).Include(p => p.User);
            return View(await gestioneBibliotecaContext.ToListAsync());
        }

        // GET: Prestiti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestito = await _context.Prestiti
                .Include(p => p.Libro)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestito == null)
            {
                return NotFound();
            }

            return View(prestito);
        }

        // GET: Prestiti/Create
        public IActionResult Create()
        {
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Utenti, "Id", "Email");
            return View();
        }

        // POST: Prestiti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataInizio,DataFine,UserId,LibroId")] Prestito prestito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Id", prestito.LibroId);
            ViewData["UserId"] = new SelectList(_context.Utenti, "Id", "Email", prestito.UserId);
            return View(prestito);
        }

        // GET: Prestiti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestito = await _context.Prestiti.FindAsync(id);
            if (prestito == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Id", prestito.LibroId);
            ViewData["UserId"] = new SelectList(_context.Utenti, "Id", "Email", prestito.UserId);
            return View(prestito);
        }

        // POST: Prestiti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataInizio,DataFine,UserId,LibroId")] Prestito prestito)
        {
            if (id != prestito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestitoExists(prestito.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libro, "Id", "Id", prestito.LibroId);
            ViewData["UserId"] = new SelectList(_context.Utenti, "Id", "Email", prestito.UserId);
            return View(prestito);
        }

        // GET: Prestiti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestito = await _context.Prestiti
                .Include(p => p.Libro)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestito == null)
            {
                return NotFound();
            }

            return View(prestito);
        }

        // POST: Prestiti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestito = await _context.Prestiti.FindAsync(id);
            if (prestito != null)
            {
                _context.Prestiti.Remove(prestito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestitoExists(int id)
        {
            return _context.Prestiti.Any(e => e.Id == id);
        }
    }
}
