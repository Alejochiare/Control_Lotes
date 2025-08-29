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
    public class CultivosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CultivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cultivos
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cultivos.ToListAsync());
        }

        // GET: Cultivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cultivos = await _context.Cultivos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cultivos == null)
            {
                return NotFound();
            }

            return View(cultivos);
        }

        // GET: Cultivos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cultivos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Semilla")] Cultivos cultivos)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(cultivos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cultivos);
        }

        // GET: Cultivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cultivos = await _context.Cultivos.FindAsync(id);
            if (cultivos == null)
            {
                return NotFound();
            }
            return View(cultivos);
        }

        // POST: Cultivos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Semilla")] Cultivos cultivos)
        {
            if (id != cultivos.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(cultivos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CultivosExists(cultivos.Id))
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
            return View(cultivos);
        }

        // GET: Cultivos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cultivos = await _context.Cultivos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cultivos == null)
            {
                return NotFound();
            }

            return View(cultivos);
        }

        // POST: Cultivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cultivos = await _context.Cultivos.FindAsync(id);
            if (cultivos != null)
            {
                _context.Cultivos.Remove(cultivos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CultivosExists(int id)
        {
            return _context.Cultivos.Any(e => e.Id == id);
        }
    }
}
