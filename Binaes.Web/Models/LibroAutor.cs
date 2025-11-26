using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class LibroAutor
    {
        [JsonPropertyName("libroId")]
        public int LibroId { get; set; }

        [JsonPropertyName("libro")]
        public Libro? Libro { get; set; }

        [JsonPropertyName("autorId")]
        public int AutorId { get; set; }

        [JsonPropertyName("autor")]
        public Autor? Autor { get; set; }
    }
}
