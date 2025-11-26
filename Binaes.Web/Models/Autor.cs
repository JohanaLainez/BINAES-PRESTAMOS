using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Autor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("libros")]
        public List<Libro>? Libros { get; set; }
    }
}
