using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _http;
        private const string Recurso = "api/Usuarios";

        public UsuariosController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi");
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _http.GetFromJsonAsync<List<Usuario>>(Recurso);
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _http.GetFromJsonAsync<Usuario>($"{Recurso}/{id}");
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create() => View();

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

          
            var payload = new
            {
                carnet = usuario.Carnet,
                nombre = usuario.Nombre,
                email = usuario.Email,
                telefono = usuario.Telefono,
                activo = usuario.Activo
            };

            var res = await _http.PostAsJsonAsync(Recurso, payload);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync(); // ver motivo real
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(usuario);
            }

            TempData["Ok"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _http.GetFromJsonAsync<Usuario>($"{Recurso}/{id}");
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();
            if (!ModelState.IsValid) return View(usuario);

           
            var payload = new
            {
                id = usuario.Id,
                carnet = usuario.Carnet,
                nombre = usuario.Nombre,
                email = usuario.Email,
                telefono = usuario.Telefono,
                activo = usuario.Activo
            };

            var res = await _http.PutAsJsonAsync($"{Recurso}/{id}", payload);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(usuario);
            }

            TempData["Ok"] = "Usuario actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _http.GetFromJsonAsync<Usuario>($"{Recurso}/{id}");
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"{Recurso}/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Usuario eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
