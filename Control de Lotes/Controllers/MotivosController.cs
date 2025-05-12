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
    public class MotivosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Motivos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Motivos.ToListAsync());
        }

        // GET: Motivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motivos = await _context.Motivos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motivos == null)
            {
                return NotFound();
            }

            return View(motivos);
        }

        // GET: Motivos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Motivos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion")] Motivos motivos)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(motivos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(motivos);
        }

        // GET: Motivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motivos = await _context.Motivos.FindAsync(id);
            if (motivos == null)
            {
                return NotFound();
            }
            return View(motivos);
        }

        // POST: Motivos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion")] Motivos motivos)
        {
            if (id != motivos.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(motivos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotivosExists(motivos.Id))
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
            return View(motivos);
        }

        // GET: Motivos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motivos = await _context.Motivos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motivos == null)
            {
                return NotFound();
            }

            return View(motivos);
        }

        // POST: Motivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motivos = await _context.Motivos.FindAsync(id);
            if (motivos != null)
            {
                _context.Motivos.Remove(motivos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotivosExists(int id)
        {
            return _context.Motivos.Any(e => e.Id == id);
        }
    }
}
