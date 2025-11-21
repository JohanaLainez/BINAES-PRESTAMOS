using Binaes.Web.Data;
using Binaes.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Binaes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AutoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            return await _context.Autores.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
                return NotFound();

            return autor;
        }
    }
}