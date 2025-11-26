using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class PrestamoItemController : Controller
    {
        private readonly HttpClient _http;

      
        private const string RecursoItems = "api/PrestamoItems";
        private const string RecursoEjemplares = "api/Ejemplares";
        private const string RecursoPrestamos = "api/Prestamos";

        public PrestamoItemController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

       
        private async Task CargarCombos(int? ejemplarId = null, int? prestamoId = null)
        {
            var ejemplares = await _http.GetFromJsonAsync<List<Ejemplar>>(RecursoEjemplares) ?? new();
            var prestamos = await _http.GetFromJsonAsync<List<Prestamo>>(RecursoPrestamos) ?? new();

           
            ViewData["EjemplarId"] = new SelectList(ejemplares, nameof(Ejemplar.Id), nameof(Ejemplar.Id), ejemplarId);
            ViewData["PrestamoId"] = new SelectList(prestamos, nameof(Prestamo.Id), nameof(Prestamo.Id), prestamoId);
        }

        // GET: PrestamoItem
        public async Task<IActionResult> Index()
        {
            var items = await _http.GetFromJsonAsync<List<PrestamoItem>>(RecursoItems);
            return View(items);
        }

        // GET: PrestamoItem/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var item = await _http.GetFromJsonAsync<PrestamoItem>($"{RecursoItems}/{id}");
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: PrestamoItem/Create
        public async Task<IActionResult> Create()
        {
            await CargarCombos();
            return View(new PrestamoItem
            {
                Renovaciones = 0
                // FechaDevolucion = null (por defecto)
            });
        }

        // POST: PrestamoItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrestamoItem item)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(item.EjemplarId, item.PrestamoId);
                return View(item);
            }

            
            var payload = new
            {
                prestamoId = item.PrestamoId,
                ejemplarId = item.EjemplarId,
                renovaciones = item.Renovaciones,
                fechaDevolucion = item.FechaDevolucion,
                estado = item.Estado
            };

            var res = await _http.PostAsJsonAsync(RecursoItems, payload);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarCombos(item.EjemplarId, item.PrestamoId);
                return View(item);
            }

            TempData["Ok"] = "Ítem de préstamo creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: PrestamoItem/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var item = await _http.GetFromJsonAsync<PrestamoItem>($"{RecursoItems}/{id}");
            if (item == null) return NotFound();

            await CargarCombos(item.EjemplarId, item.PrestamoId);
            return View(item);
        }

        // POST: PrestamoItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, PrestamoItem item)
        {
            if (id != item.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarCombos(item.EjemplarId, item.PrestamoId);
                return View(item);
            }

            var payload = new
            {
                id = item.Id,
                prestamoId = item.PrestamoId,
                ejemplarId = item.EjemplarId,
                renovaciones = item.Renovaciones,
                fechaDevolucion = item.FechaDevolucion,
                estado = item.Estado
            };

            var res = await _http.PutAsJsonAsync($"{RecursoItems}/{id}", payload);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarCombos(item.EjemplarId, item.PrestamoId);
                return View(item);
            }

            TempData["Ok"] = "Ítem de préstamo actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: PrestamoItem/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var item = await _http.GetFromJsonAsync<PrestamoItem>($"{RecursoItems}/{id}");
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: PrestamoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var res = await _http.DeleteAsync($"{RecursoItems}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Ítem eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
