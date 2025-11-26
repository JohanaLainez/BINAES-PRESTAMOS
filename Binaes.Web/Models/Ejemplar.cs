using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Ejemplar
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("libroId")]
        public int LibroId { get; set; }

        [JsonPropertyName("libro")]
        public Libro? Libro { get; set; }

        [JsonPropertyName("codigoInterno")]
        public string? CodigoInterno { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("prestamoItems")]
        public List<PrestamoItem>? PrestamoItems { get; set; }
    }
}
