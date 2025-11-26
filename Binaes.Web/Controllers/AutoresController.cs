using Binaes.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Binaes.Web.Controllers
{
    public class AutoresController : Controller
    {
        private readonly HttpClient _http;

        public AutoresController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

        // GET: Autores
        public async Task<IActionResult> Index()
        {
            var autores = await _http.GetFromJsonAsync<List<Autor>>("/api/Autores");
            return View(autores);
        }

        // GET: Autores/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var autor = await _http.GetFromJsonAsync<Autor>($"/api/Autores/{id}");
            if (autor == null) return NotFound();
            return View(autor);
        }

        // GET: Autores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        [HttpPost]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (!ModelState.IsValid) return View(autor);

            var response = await _http.PostAsJsonAsync("/api/Autores", autor);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al guardar.");
                return View(autor);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Autores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var autor = await _http.GetFromJsonAsync<Autor>($"/api/Autores/{id}");
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Autores/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            if (id != autor.Id) return NotFound();
            if (!ModelState.IsValid) return View(autor);

            var response = await _http.PutAsJsonAsync($"/api/Autores/{id}", autor);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Error al actualizar.");
                return View(autor);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Autores/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var autor = await _http.GetFromJsonAsync<Autor>($"/api/Autores/{id}");
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _http.DeleteAsync($"/api/Autores/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
