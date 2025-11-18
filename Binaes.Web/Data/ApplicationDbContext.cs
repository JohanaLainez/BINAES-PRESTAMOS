using Microsoft.EntityFrameworkCore;
using Binaes.Web.Models;

namespace Binaes.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<LibroAutor> LibroAutores { get; set; }
        public DbSet<Ejemplar> Ejemplares { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Parametros> Parametros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<PrestamoItem> PrestamoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LibroAutor>()
                .HasKey(la => new { la.LibroId, la.AutorId });
        }
    }
}
