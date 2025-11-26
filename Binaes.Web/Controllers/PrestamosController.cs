using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly HttpClient _http;
        private const string Recurso = "api/Prestamos";

        public PrestamosController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi"); 
        }

        
        private async Task CargarCombosAsync(int? usuarioId = null, int? staffId = null)
        {
            // ⚠️ Staff en SINGULAR
            var usuarios = await _http.GetFromJsonAsync<List<Usuario>>("api/Usuarios") ?? new();
            var staffs = await _http.GetFromJsonAsync<List<Staff>>("api/Staff") ?? new();

            ViewData["UsuarioId"] = new SelectList(usuarios, nameof(Usuario.Id), nameof(Usuario.Nombre), usuarioId);
            ViewData["StaffId"] = new SelectList(staffs, nameof(Staff.Id), nameof(Staff.Nombre), staffId);
        }

        
        public async Task<IActionResult> Index()
        {
            var prestamos = await _http.GetFromJsonAsync<List<Prestamo>>(Recurso);
            return View(prestamos);
        }

     
        public async Task<IActionResult> Details(int id)
        {
            var prestamo = await _http.GetFromJsonAsync<Prestamo>($"{Recurso}/{id}");
            if (prestamo is null) return NotFound();
            return View(prestamo);
        }

        
        public async Task<IActionResult> Create()
        {
            await CargarCombosAsync();
            return View(new Prestamo
            {
                FechaPrestamo = DateTime.Today,
                FechaVencimiento = DateTime.Today.AddDays(7)
            });
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(prestamo.UsuarioId, prestamo.StaffId);
                return View(prestamo);
            }

            
            var payload = new
            {
                usuarioId = prestamo.UsuarioId,
                staffId = prestamo.StaffId,
                fechaPrestamo = prestamo.FechaPrestamo,
                fechaVencimiento = prestamo.FechaVencimiento,
                fechaDevolucion = prestamo.FechaDevolucion,
                estado = prestamo.Estado
            };

            var res = await _http.PostAsJsonAsync(Recurso, payload);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarCombosAsync(prestamo.UsuarioId, prestamo.StaffId);
                return View(prestamo);
            }

            TempData["Ok"] = "Préstamo creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var prestamo = await _http.GetFromJsonAsync<Prestamo>($"{Recurso}/{id}");
            if (prestamo is null) return NotFound();

            await CargarCombosAsync(prestamo.UsuarioId, prestamo.StaffId);
            return View(prestamo);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prestamo prestamo)
        {
            if (id != prestamo.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(prestamo.UsuarioId, prestamo.StaffId);
                return View(prestamo);
            }

            var payload = new
            {
                id = prestamo.Id,
                usuarioId = prestamo.UsuarioId,
                staffId = prestamo.StaffId,
                fechaPrestamo = prestamo.FechaPrestamo,
                fechaVencimiento = prestamo.FechaVencimiento,
                fechaDevolucion = prestamo.FechaDevolucion,
                estado = prestamo.Estado
            };

            var res = await _http.PutAsJsonAsync($"{Recurso}/{id}", payload);
            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                await CargarCombosAsync(prestamo.UsuarioId, prestamo.StaffId);
                return View(prestamo);
            }

            TempData["Ok"] = "Préstamo actualizado.";
            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var prestamo = await _http.GetFromJsonAsync<Prestamo>($"{Recurso}/{id}");
            if (prestamo is null) return NotFound();
            return View(prestamo);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{Recurso}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Préstamo eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
