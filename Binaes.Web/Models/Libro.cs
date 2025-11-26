using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Libro
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("isbn")]
        public string? Isbn { get; set; }

        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("anioPublicacion")]
        public int AnioPublicacion { get; set; }

        [JsonPropertyName("categoriaId")]
        public int CategoriaId { get; set; }

        [JsonPropertyName("categoria")]
        public Categoria? Categoria { get; set; }

        [JsonPropertyName("autores")]
        public List<Autor>? Autores { get; set; }

        [JsonPropertyName("ejemplares")]
        public List<Ejemplar>? Ejemplares { get; set; }
    }
}
