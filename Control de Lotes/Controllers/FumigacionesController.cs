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
        public async Task<IActionResult> Index(int? loteId, string aplicador)
        {
            // Obtener lista de lotes para los filtros
            ViewData["Lotes"] = new SelectList(await _context.Lote.ToListAsync(), "Id", "Nombre");

            // Construir la consulta inicial
            var fumigaciones = _context.Fumigacion
                                        .Include(f => f.Lote)
                                        .Include(f => f.Motivo)
                                        .AsQueryable();

            // Filtrar por lote si está presente
            if (loteId.HasValue)
            {
                fumigaciones = fumigaciones.Where(f => f.LoteId == loteId.Value);
            }

            // Filtrar por aplicador si está presente, convirtiendo todo a minúsculas
            if (!string.IsNullOrEmpty(aplicador))
            {
                fumigaciones = fumigaciones.Where(f => EF.Functions.Like(f.Aplicador.ToLower(), aplicador.ToLower() + "%"));
            }

            return View(await fumigaciones.ToListAsync());
        }




        // GET: Fumigaciones/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Fumigaciones/Create
        public IActionResult Create()
        {
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Nombre");
            ViewData["MotivoId"] = new SelectList(_context.Set<Motivos>(), "Id", "Descripcion");
            return View();
        }

        // POST: Fumigaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Fumigaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fumigacion = await _context.Fumigacion.FindAsync(id);
            if (fumigacion == null)
            {
                return NotFound();
            }
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Id", fumigacion.LoteId);
            ViewData["MotivoId"] = new SelectList(_context.Set<Motivos>(), "Id", "Id", fumigacion.MotivoId);
            return View(fumigacion);
        }

        // POST: Fumigaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Producto,Dosis,Aplicador,LoteId,MotivoId")] Fumigacion fumigacion)
        {
            if (id != fumigacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fumigacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FumigacionExists(fumigacion.Id))
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
            ViewData["LoteId"] = new SelectList(_context.Set<Lote>(), "Id", "Id", fumigacion.LoteId);
            ViewData["MotivoId"] = new SelectList(_context.Set<Motivos>(), "Id", "Id", fumigacion.MotivoId);
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

        private bool FumigacionExists(int id)
        {
            return _context.Fumigacion.Any(e => e.Id == id);
        }


        public IActionResult SoporteTecnico(string msj, string numero)
        {
            // Creamos la URL de WhatsApp
            string urlWhatsApp = $"https://api.whatsapp.com/send?phone={numero}&text={Uri.EscapeDataString(msj)}";

            // En vez de hacer un redirect, solo pasamos la URL a la vista
            TempData["WhatsAppUrl"] = urlWhatsApp;

            // Luego redirigimos a la vista donde lo vamos a manejar
            return RedirectToAction("Index");  // O la vista que desees
        }
    }
}
