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
    public class SiembrasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SiembrasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Siembras
        [Authorize]
        public async Task<IActionResult> Index(int? loteId, int? cultivoId, int page = 1, int pageSize = 5)
        {
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");

            var siembrasQuery = _context.Siembra
                .Include(s => s.cultivo)
                .Include(s => s.Lote)
                .AsQueryable();

            if (loteId.HasValue)
                siembrasQuery = siembrasQuery.Where(s => s.LoteId == loteId);

            if (cultivoId.HasValue)
                siembrasQuery = siembrasQuery.Where(s => s.CultivoId == cultivoId);

            var totalSiembras = await siembrasQuery.CountAsync();

            var siembras = await siembrasQuery
                .OrderByDescending(s => s.Fecha) // opcional
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalSiembras / (double)pageSize);

            return View(siembras);
        }

        // GET: Siembras/Create
        public IActionResult Create()
        {
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            ViewData["Destino"] = new SelectList(new List<string> { "Picado", "Cosecha" });

            return View();
        }

        // POST: Siembras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,ProduccionEstimativa,CantidadSemillas,Superficiesembrada,LoteId,CultivoId,Destino")]Siembra siembra)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(siembra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla", siembra.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre", siembra.LoteId);
            ViewData["Destino"] = new SelectList(new List<string> { "Picado", "Cosecha" }, siembra.Destino);
            return View(siembra);
        }

        // GET: Siembras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siembra = await _context.Siembra
                .Include(s => s.cultivo)
                .Include(s => s.Lote)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siembra == null)
            {
                return NotFound();
            }

            return View(siembra);
        }

        // POST: Siembras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var siembra = await _context.Siembra.FindAsync(id);
            if (siembra != null)
            {
                _context.Siembra.Remove(siembra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiembraExists(int id)
        {
            return _context.Siembra.Any(e => e.Id == id);
        }
    }
}
