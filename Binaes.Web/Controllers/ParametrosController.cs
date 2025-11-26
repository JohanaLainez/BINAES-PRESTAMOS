using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class ParametrosController : Controller
    {
        private readonly HttpClient _http;
        private const string Recurso = "api/Parametros";

        public ParametrosController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

        // GET: /Parametros
        public async Task<IActionResult> Index()
        {
            var parametros = await _http.GetFromJsonAsync<List<Parametro>>(Recurso);
            return View(parametros);
        }

        // GET: /Parametros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var parametro = await _http.GetFromJsonAsync<Parametro>($"{Recurso}/{id}");
            if (parametro == null) return NotFound();
            return View(parametro);
        }

        // GET: /Parametros/Create
        public IActionResult Create() => View();

        // POST: /Parametros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Parametro parametro)
        {
            if (!ModelState.IsValid) return View(parametro);

            var res = await _http.PostAsJsonAsync(Recurso, parametro);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(parametro);
            }

            TempData["Ok"] = "Parámetro creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Parametros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var parametro = await _http.GetFromJsonAsync<Parametro>($"{Recurso}/{id}");
            if (parametro == null) return NotFound();
            return View(parametro);
        }

        // POST: /Parametros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Parametro parametro)
        {
            if (id != parametro.Id) return BadRequest();
            if (!ModelState.IsValid) return View(parametro);

            var res = await _http.PutAsJsonAsync($"{Recurso}/{id}", parametro);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(parametro);
            }

            TempData["Ok"] = "Parámetro actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Parametros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var parametro = await _http.GetFromJsonAsync<Parametro>($"{Recurso}/{id}");
            if (parametro == null) return NotFound();
            return View(parametro);
        }

        // POST: /Parametros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{Recurso}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Parámetro eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
