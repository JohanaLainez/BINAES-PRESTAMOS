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
    public class EjemplaresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EjemplaresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ejemplares
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ejemplares.Include(e => e.Libro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ejemplares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ejemplar = await _context.Ejemplares
                .Include(e => e.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ejemplar == null)
            {
                return NotFound();
            }

            return View(ejemplar);
        }

        // GET: Ejemplares/Create
        public IActionResult Create()
        {
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id");
            return View();
        }

        // POST: Ejemplares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LibroId,CodigoInterno,Estado")] Ejemplar ejemplar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ejemplar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", ejemplar.LibroId);
            return View(ejemplar);
        }

        // GET: Ejemplares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ejemplar = await _context.Ejemplares.FindAsync(id);
            if (ejemplar == null)
            {
                return NotFound();
            }
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", ejemplar.LibroId);
            return View(ejemplar);
        }

        // POST: Ejemplares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LibroId,CodigoInterno,Estado")] Ejemplar ejemplar)
        {
            if (id != ejemplar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ejemplar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EjemplarExists(ejemplar.Id))
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
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", ejemplar.LibroId);
            return View(ejemplar);
        }

        // GET: Ejemplares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ejemplar = await _context.Ejemplares
                .Include(e => e.Libro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ejemplar == null)
            {
                return NotFound();
            }

            return View(ejemplar);
        }

        // POST: Ejemplares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ejemplar = await _context.Ejemplares.FindAsync(id);
            if (ejemplar != null)
            {
                _context.Ejemplares.Remove(ejemplar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EjemplarExists(int id)
        {
            return _context.Ejemplares.Any(e => e.Id == id);
        }
    }
}
