using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly HttpClient _http;

        private const string Recurso = "api/Categorias";

        public CategoriasController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi"); 
        }

        // GET: /Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _http.GetFromJsonAsync<List<Categoria>>(Recurso) ?? new();
            return View(categorias);
        }

        // GET: /Categorias/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _http.GetFromJsonAsync<Categoria>($"{Recurso}/{id}");
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // GET: /Categorias/Create
        public IActionResult Create() => View();

        // POST: /Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (!ModelState.IsValid) return View(categoria);

            var res = await _http.PostAsJsonAsync(Recurso, categoria);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(categoria);
            }

            TempData["Ok"] = "Categoría creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Categorias/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _http.GetFromJsonAsync<Categoria>($"{Recurso}/{id}");
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // POST: /Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            if (!ModelState.IsValid) return View(categoria);

            var res = await _http.PutAsJsonAsync($"{Recurso}/{id}", categoria);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(categoria);
            }

            TempData["Ok"] = "Categoría actualizada.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Categorias/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _http.GetFromJsonAsync<Categoria>($"{Recurso}/{id}");
            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // POST: /Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{Recurso}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Categoría eliminada." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
