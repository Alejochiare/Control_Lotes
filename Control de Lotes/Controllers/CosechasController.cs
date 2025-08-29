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
    public class CosechasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CosechasController(ApplicationDbContext context)
        {
            _context = context;
        }
	 
		// GET: Cosechas
		[HttpGet]
		[Authorize]
        public async Task<IActionResult> Index(int? loteId, int? cultivoId, int page = 1, int pageSize = 5)
        {
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");

            var cosechas = _context.Cosecha
                         .Include(c => c.cultivo)
                         .Include(c => c.Lote)
                         .AsQueryable();

            if (loteId.HasValue)
            {
                cosechas = cosechas.Where(c => c.LoteId == loteId.Value);
            }

            if (cultivoId.HasValue)
            {
                cosechas = cosechas.Where(c => c.CultivoId == cultivoId.Value);
            }

            var totalcosecha = await cosechas.CountAsync();

            var cosechasPaginadas = await cosechas
                .OrderByDescending(s => s.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalcosecha / (double)pageSize);

            return View(cosechasPaginadas);
        }

        // GET: Cosechas/Create
        public IActionResult Create()
        {
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            return View();
        }

        // POST: Cosechas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ProduccionReal,LoteId,CultivoId")] Cosecha cosecha)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(cosecha);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla", cosecha.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre", cosecha.LoteId);
            return View(cosecha);
        }

        // GET: Cosechas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosecha = await _context.Cosecha
                .Include(c => c.cultivo)
                .Include(c => c.Lote)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cosecha == null)
            {
                return NotFound();
            }

            return View(cosecha);
        }

        // POST: Cosechas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cosecha = await _context.Cosecha.FindAsync(id);
            if (cosecha != null)
            {
                _context.Cosecha.Remove(cosecha);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CosechaExists(int id)
        {
            return _context.Cosecha.Any(e => e.Id == id);
        }
    }
}
