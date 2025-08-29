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
    public class FumigacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FumigacionesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Fumigaciones
        [Authorize]
        public async Task<IActionResult> Index(int? loteId, string aplicador, int page = 1, int pageSize = 5)
        {

            ViewData["Lotes"] = new SelectList(await _context.Lote.ToListAsync(), "Id", "Nombre");

            var fumigaciones = _context.Fumigacion
                               .Include(f => f.Lote)
                               .Include(f => f.Motivo)
                               .AsQueryable();

            if (loteId.HasValue)
            {
                fumigaciones = fumigaciones.Where(f => f.LoteId == loteId.Value);
            }

            if (!string.IsNullOrEmpty(aplicador))
            {
                fumigaciones = fumigaciones.Where(f => EF.Functions.Like(f.Aplicador.ToLower(), aplicador.ToLower() + "%"));
            }

            var totalFumigaciones = await fumigaciones.CountAsync();
            var fumigacionesPaginadas = await fumigaciones
                .OrderByDescending(f => f.Fecha) // asegúrate que tienes una propiedad Fecha
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalFumigaciones / (double)pageSize);

            return View(fumigacionesPaginadas);
        }

        // GET: Fumigaciones/Create
        public IActionResult Create()
        {
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Nombre");
            ViewData["MotivoId"] = new SelectList(_context.Set<Motivos>(), "Id", "Descripcion");
            return View();
        }

        // POST: Fumigaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Producto,Dosis,Aplicador,LoteId,MotivoId")] Fumigacion fumigacion)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(fumigacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Nombre", fumigacion.LoteId);
            ViewData["MotivoId"] = new SelectList(_context.Set<Motivos>(), "Id", "Descripcion", fumigacion.MotivoId);
            return View(fumigacion);
        }

        // GET: Fumigaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fumigacion = await _context.Fumigacion
                .Include(f => f.Lote)
                .Include(f => f.Motivo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fumigacion == null)
            {
                return NotFound();
            }

            return View(fumigacion);
        }

        // POST: Fumigaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fumigacion = await _context.Fumigacion.FindAsync(id);
            if (fumigacion != null)
            {
                _context.Fumigacion.Remove(fumigacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
