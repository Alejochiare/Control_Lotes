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
        public async Task<IActionResult> Index(int? loteId, int? cultivoId)
        {
            // Llenar las listas desplegables de Lote y Cultivo
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");

            // Consulta inicial
            var siembras = _context.Siembra.Include(s => s.cultivo).Include(s => s.Lote).AsQueryable();

            // Filtrar según los parámetros
            if (loteId.HasValue)
            {
                siembras = siembras.Where(s => s.LoteId == loteId);
            }

            if (cultivoId.HasValue)
            {
                siembras = siembras.Where(s => s.CultivoId == cultivoId);
            }

            return View(await siembras.ToListAsync());
        }

        // GET: Siembras/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Siembras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siembra = await _context.Siembra.FindAsync(id);
            if (siembra == null)
            {
                return NotFound();
            }
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla", siembra.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre", siembra.LoteId);
            return View(siembra);
        }

        // POST: Siembras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ProduccionEstimativa,CantidadSemillas,Superficiesembrada,LoteId,CultivoId")] Siembra siembra)
        {
            if (id != siembra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(siembra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiembraExists(siembra.Id))
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
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla", siembra.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre", siembra.LoteId);
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
