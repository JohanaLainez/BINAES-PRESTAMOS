using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Binaes.Web.Models;

namespace Binaes.Web.Controllers
{
    public class StaffsController : Controller
    {
        private readonly HttpClient _http;

        public StaffsController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("BinaesApi"); 
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var staffList = await _http.GetFromJsonAsync<List<Staff>>("api/Staff");
            return View(staffList);
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var staff = await _http.GetFromJsonAsync<Staff>($"api/Staff/{id}");
            if (staff == null) return NotFound();
            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create() => View();

        // POST: Staffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Staff staff)
        {
            if (!ModelState.IsValid) return View(staff);

            var res = await _http.PostAsJsonAsync("api/Staff", staff);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();   // <-- ver motivo real
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(staff);
            }

            TempData["Ok"] = "Staff creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var staff = await _http.GetFromJsonAsync<Staff>($"api/Staff/{id}");
            if (staff == null) return NotFound();
            return View(staff);
        }

        // POST: Staffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Staff staff)
        {
            if (id != staff.Id) return BadRequest();
            if (!ModelState.IsValid) return View(staff);

            var res = await _http.PutAsJsonAsync($"api/Staff/{id}", staff);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();   // <-- ver motivo real
                ModelState.AddModelError("", $"Error {(int)res.StatusCode}: {body}");
                return View(staff);
            }

            TempData["Ok"] = "Staff actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _http.GetFromJsonAsync<Staff>($"api/Staff/{id}");
            if (staff == null) return NotFound();
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var res = await _http.DeleteAsync($"api/Staff/{id}");
            TempData[res.IsSuccessStatusCode ? "Ok" : "Error"] =
                res.IsSuccessStatusCode ? "Staff eliminado." : $"Error {(int)res.StatusCode}";
            return RedirectToAction(nameof(Index));
        }
    }
}
