using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class PrestamoItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("prestamoId")]
        public int PrestamoId { get; set; }

        [JsonPropertyName("prestamo")]
        public Prestamo? Prestamo { get; set; }

        [JsonPropertyName("ejemplarId")]
        public int EjemplarId { get; set; }

        [JsonPropertyName("ejemplar")]
        public Ejemplar? Ejemplar { get; set; }

        [JsonPropertyName("renovaciones")]
        public int Renovaciones { get; set; }

        [JsonPropertyName("fechaDevolucion")]
        public DateTime? FechaDevolucion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }
    }
}
