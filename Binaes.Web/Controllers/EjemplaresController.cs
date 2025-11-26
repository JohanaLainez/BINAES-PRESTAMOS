using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class EjemplaresController : Controller
    {
        private readonly HttpClient _http;

        private const string RecursoEjemplares = "api/Ejemplares";
        private const string RecursoLibros = "api/Libros";

        public EjemplaresController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi"); 
        }

        // GET: /Ejemplares
        public async Task<IActionResult> Index()
        {
            var ejemplares = await _http.GetFromJsonAsync<List<Ejemplar>>(RecursoEjemplares) ?? new();
            return View(ejemplares);
        }

        // GET: /Ejemplares/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ejemplar = await _http.GetFromJsonAsync<Ejemplar>($"{RecursoEjemplares}/{id}");
            if (ejemplar == null) return NotFound();
            return View(ejemplar);
        }

       
        private async Task CargarLibrosSelectAsync(int? seleccionado = null)
        {
            var libros = await _http.GetFromJsonAsync<List<Libro>>(RecursoLibros) ?? new();
            ViewData["LibroId"] = new SelectList(libros, nameof(Libro.Id), nameof(Libro.Titulo), seleccionado);
        }

        // GET: /Ejemplares/Create
        public async Task<IActionResult> Create()
        {
            await CargarLibrosSelectAsync();
            return View();
        }

        // POST: /Ejemplares/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ejemplar ejemplar)
        {
            if (!ModelState.IsValid)
            {
                await CargarLibrosSelectAsync(ejemplar.LibroId);
                return View(ejemplar);
            }

            var res = await _http.PostAsJsonAsync(RecursoEjemplares, ejemplar);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarLibrosSelectAsync(ejemplar.LibroId);
                return View(ejemplar);
            }

            TempData["Ok"] = "Ejemplar creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ejemplares/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ejemplar = await _http.GetFromJsonAsync<Ejemplar>($"{RecursoEjemplares}/{id}");
            if (ejemplar == null) return NotFound();

            await CargarLibrosSelectAsync(ejemplar.LibroId);
            return View(ejemplar);
        }

        // POST: /Ejemplares/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ejemplar ejemplar)
        {
            if (id != ejemplar.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarLibrosSelectAsync(ejemplar.LibroId);
                return View(ejemplar);
            }

            var res = await _http.PutAsJsonAsync($"{RecursoEjemplares}/{id}", ejemplar);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarLibrosSelectAsync(ejemplar.LibroId);
                return View(ejemplar);
            }

            TempData["Ok"] = "Ejemplar actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Ejemplares/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ejemplar = await _http.GetFromJsonAsync<Ejemplar>($"{RecursoEjemplares}/{id}");
            if (ejemplar == null) return NotFound();
            return View(ejemplar);
        }

        // POST: /Ejemplares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{RecursoEjemplares}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Ejemplar eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
