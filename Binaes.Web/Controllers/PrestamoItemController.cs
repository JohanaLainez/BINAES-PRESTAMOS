using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Binaes.Web.Data;

namespace Binaes.Web.Controllers
{
    public class PrestamoItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrestamoItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrestamoItem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrestamoItems.Include(p => p.Ejemplar).Include(p => p.Prestamo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrestamoItem/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamoItem = await _context.PrestamoItems
                .Include(p => p.Ejemplar)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamoItem == null)
            {
                return NotFound();
            }

            return View(prestamoItem);
        }

        // GET: PrestamoItem/Create
        public IActionResult Create()
        {
            ViewData["EjemplarId"] = new SelectList(_context.Ejemplares, "Id", "Id");
            ViewData["PrestamoId"] = new SelectList(_context.Prestamos, "Id", "Id");
            return View();
        }

        // POST: PrestamoItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrestamoId,EjemplarId,Renovaciones,FechaDevolucion,Estado")] PrestamoItem prestamoItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EjemplarId"] = new SelectList(_context.Ejemplares, "Id", "Id", prestamoItem.EjemplarId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamos, "Id", "Id", prestamoItem.PrestamoId);
            return View(prestamoItem);
        }

        // GET: PrestamoItem/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamoItem = await _context.PrestamoItems.FindAsync(id);
            if (prestamoItem == null)
            {
                return NotFound();
            }
            ViewData["EjemplarId"] = new SelectList(_context.Ejemplares, "Id", "Id", prestamoItem.EjemplarId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamos, "Id", "Id", prestamoItem.PrestamoId);
            return View(prestamoItem);
        }

        // POST: PrestamoItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,PrestamoId,EjemplarId,Renovaciones,FechaDevolucion,Estado")] PrestamoItem prestamoItem)
        {
            if (id != prestamoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamoItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoItemExists(prestamoItem.Id))
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
            ViewData["EjemplarId"] = new SelectList(_context.Ejemplares, "Id", "Id", prestamoItem.EjemplarId);
            ViewData["PrestamoId"] = new SelectList(_context.Prestamos, "Id", "Id", prestamoItem.PrestamoId);
            return View(prestamoItem);
        }

        // GET: PrestamoItem/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamoItem = await _context.PrestamoItems
                .Include(p => p.Ejemplar)
                .Include(p => p.Prestamo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamoItem == null)
            {
                return NotFound();
            }

            return View(prestamoItem);
        }

        // POST: PrestamoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var prestamoItem = await _context.PrestamoItems.FindAsync(id);
            if (prestamoItem != null)
            {
                _context.PrestamoItems.Remove(prestamoItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoItemExists(long id)
        {
            return _context.PrestamoItems.Any(e => e.Id == id);
        }
    }
}
