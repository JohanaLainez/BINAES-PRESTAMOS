using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly HttpClient _http;
        private const string RecursoLibros = "api/Libros";
        private const string RecursoCategorias = "api/Categorias";

        public LibrosController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

        public async Task<IActionResult> Index()
        {
            var libros = await _http.GetFromJsonAsync<List<Libro>>(RecursoLibros);
            return View(libros);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _http.GetFromJsonAsync<Libro>($"{RecursoLibros}/{id}");
            if (libro == null) return NotFound();
            return View(libro);
        }

        public async Task<IActionResult> Create()
        {
            await CargarCategorias();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                await CargarCategorias();
                return View(libro);
            }

            var res = await _http.PostAsJsonAsync(RecursoLibros, libro);

            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {error}");
                await CargarCategorias();
                return View(libro);
            }

            TempData["Ok"] = "Libro creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _http.GetFromJsonAsync<Libro>($"{RecursoLibros}/{id}");
            if (libro == null) return NotFound();

            await CargarCategorias(libro.CategoriaId);
            return View(libro);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarCategorias(libro.CategoriaId);
                return View(libro);
            }

            var res = await _http.PutAsJsonAsync($"{RecursoLibros}/{id}", libro);

            if (!res.IsSuccessStatusCode)
            {
                var error = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {error}");
                await CargarCategorias(libro.CategoriaId);
                return View(libro);
            }

            TempData["Ok"] = "Libro actualizado.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _http.GetFromJsonAsync<Libro>($"{RecursoLibros}/{id}");
            if (libro == null) return NotFound();

            return View(libro);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{RecursoLibros}/{id}");

            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Libro eliminado." : $"Error {(int)res.StatusCode}";

            return RedirectToAction(nameof(Index));
        }

        
        private async Task CargarCategorias(int? seleccion = null)
        {
            var categorias = await _http.GetFromJsonAsync<List<Categoria>>(RecursoCategorias) ?? new();

            ViewData["Categorias"] = new SelectList(
                categorias,
                nameof(Categoria.Id),
                nameof(Categoria.Nombre),
                seleccion
            );
        }
    }
}
