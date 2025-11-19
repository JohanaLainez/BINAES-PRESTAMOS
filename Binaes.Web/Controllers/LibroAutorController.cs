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
    public class LibroAutorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibroAutorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LibroAutor
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.LibroAutores.Include(l => l.Autor).Include(l => l.Libro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LibroAutor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libroAutor = await _context.LibroAutores
                .Include(l => l.Autor)
                .Include(l => l.Libro)
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libroAutor == null)
            {
                return NotFound();
            }

            return View(libroAutor);
        }

        // GET: LibroAutor/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Id");
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id");
            return View();
        }

        // POST: LibroAutor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibroId,AutorId")] LibroAutor libroAutor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libroAutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Id", libroAutor.AutorId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", libroAutor.LibroId);
            return View(libroAutor);
        }

        // GET: LibroAutor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libroAutor = await _context.LibroAutores.FindAsync(id);
            if (libroAutor == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Id", libroAutor.AutorId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", libroAutor.LibroId);
            return View(libroAutor);
        }

        // POST: LibroAutor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibroId,AutorId")] LibroAutor libroAutor)
        {
            if (id != libroAutor.LibroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libroAutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroAutorExists(libroAutor.LibroId))
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
            ViewData["AutorId"] = new SelectList(_context.Autores, "Id", "Id", libroAutor.AutorId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "Id", "Id", libroAutor.LibroId);
            return View(libroAutor);
        }

        // GET: LibroAutor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libroAutor = await _context.LibroAutores
                .Include(l => l.Autor)
                .Include(l => l.Libro)
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libroAutor == null)
            {
                return NotFound();
            }

            return View(libroAutor);
        }

        // POST: LibroAutor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libroAutor = await _context.LibroAutores.FindAsync(id);
            if (libroAutor != null)
            {
                _context.LibroAutores.Remove(libroAutor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroAutorExists(int id)
        {
            return _context.LibroAutores.Any(e => e.LibroId == id);
        }
    }
}
