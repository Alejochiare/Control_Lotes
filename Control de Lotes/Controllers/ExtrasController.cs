using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Control_de_Lotes.Data;
using Control_de_Lotes.Models;
using Microsoft.AspNetCore.Authorization;

namespace Control_de_Lotes.Controllers
{
    public class ExtrasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExtrasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Extras
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Extras.Include(e => e.Lote);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Extras/Create
        public IActionResult Create()
        {
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Nombre");
            return View();
        }

        // POST: Extras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nota,LoteId,Fecha")] Extras extras)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(extras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Nombre", extras.LoteId);
            return View(extras);
        }
       
        // GET: Extras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extras = await _context.Extras
                .Include(e => e.Lote)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extras == null)
            {
                return NotFound();
            }

            return View(extras);
        }

        // POST: Extras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var extras = await _context.Extras.FindAsync(id);
            if (extras != null)
            {
                _context.Extras.Remove(extras);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtrasExists(int id)
        {
            return _context.Extras.Any(e => e.Id == id);
        }
    }
}
