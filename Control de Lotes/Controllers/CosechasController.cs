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
        [Authorize]
        public async Task<IActionResult> Index(int? loteId, int? cultivoId)
        {
            // Obtén los lotes y cultivos para llenar las listas desplegables
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");

            // Construir la consulta inicial
            var cosechas = _context.Cosecha
                         .Include(c => c.cultivo)  // Incluye la propiedad cultivo
                         .Include(c => c.Lote)     // Incluye la propiedad Lote
                         .AsQueryable();          // Esto se asegura de que la consulta sea del tipo correcto


            // Aplicar filtros si los parámetros están presentes
            if (loteId.HasValue)
            {
                cosechas = cosechas.Where(c => c.LoteId == loteId);
            }

            if (cultivoId.HasValue)
            {
                cosechas = cosechas.Where(c => c.CultivoId == cultivoId);
            }

            return View(await cosechas.ToListAsync());
        }


        // GET: Cosechas/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Cosechas/Create
        public IActionResult Create()
        {
            ViewData["CultivoId"] = new SelectList(_context.Cultivos, "Id", "Semilla");
            ViewData["LoteId"] = new SelectList(_context.Lote, "Id", "Nombre");
            return View();
        }

        // POST: Cosechas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Cosechas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cosecha = await _context.Cosecha.FindAsync(id);
            if (cosecha == null)
            {
                return NotFound();
            }
            ViewData["CultivoId"] = new SelectList(_context.Set<Cultivos>(), "Id", "Id", cosecha.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Id", cosecha.LoteId);
            return View(cosecha);
        }

        // POST: Cosechas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,ProduccionReal,LoteId,CultivoId")] Cosecha cosecha)
        {
            if (id != cosecha.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cosecha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CosechaExists(cosecha.Id))
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
            ViewData["CultivoId"] = new SelectList(_context.Set<Cultivos>(), "Id", "Id", cosecha.CultivoId);
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Id", cosecha.LoteId);
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
