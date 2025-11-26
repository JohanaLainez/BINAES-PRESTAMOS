using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class LibroAutorController : Controller
    {
        private readonly HttpClient _http;
        private const string Recurso = "api/LibroAutores"; 

        public LibroAutorController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

        // GET: LibroAutor
        public async Task<IActionResult> Index()
        {
            var lista = await _http.GetFromJsonAsync<List<LibroAutor>>(Recurso) ?? new();
            return View(lista);
        }

        // GET: LibroAutor/Create
        public async Task<IActionResult> Create()
        {
            await RellenarCombos();
            return View();
        }

        // POST: LibroAutor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibroAutor model)
        {
            if (!ModelState.IsValid)
            {
                await RellenarCombos(model.AutorId, model.LibroId);
                return View(model);
            }

          
            var payload = new
            {
                autorId = model.AutorId,
                libroId = model.LibroId
            };

            var res = await _http.PostAsJsonAsync(Recurso, payload);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await RellenarCombos(model.AutorId, model.LibroId);
                return View(model);
            }

            TempData["Ok"] = "Relación Libro–Autor creada.";
            return RedirectToAction(nameof(Index));
        }

        // GET: LibroAutor/Delete?libroId=1&autorId=2
        public IActionResult Delete(int libroId, int autorId)
        {
           
            return View(new LibroAutor { LibroId = libroId, AutorId = autorId });
        }

        // POST: LibroAutor/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int libroId, int autorId)
        {
            // Variante común: DELETE con querystring
            var res = await _http.DeleteAsync($"{Recurso}?libroId={libroId}&autorId={autorId}");

            // Fallback: algunas APIs aceptan DELETE con body
            if (!res.IsSuccessStatusCode)
            {
                var req = new HttpRequestMessage(HttpMethod.Delete, Recurso)
                {
                    Content = JsonContent.Create(new { libroId, autorId })
                };
                res = await _http.SendAsync(req);
            }

            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Relación eliminada." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }

        private async Task RellenarCombos(int? autorId = null, int? libroId = null)
        {
            var autores = await _http.GetFromJsonAsync<List<Autor>>("api/Autores") ?? new();
            var libros = await _http.GetFromJsonAsync<List<Libro>>("api/Libros") ?? new();

            ViewData["AutorId"] = new SelectList(autores, nameof(Autor.Id), nameof(Autor.Nombre), autorId);
            ViewData["LibroId"] = new SelectList(libros, nameof(Libro.Id), nameof(Libro.Titulo), libroId);
        }
    }
}
