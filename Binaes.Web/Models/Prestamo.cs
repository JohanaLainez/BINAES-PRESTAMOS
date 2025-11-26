using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Prestamo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("usuario")]
        public Usuario? Usuario { get; set; }

        [JsonPropertyName("staffId")]
        public int StaffId { get; set; }

        [JsonPropertyName("staff")]
        public Staff? Staff { get; set; }

        [JsonPropertyName("fechaPrestamo")]
        public DateTime FechaPrestamo { get; set; }

        [JsonPropertyName("fechaVencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [JsonPropertyName("fechaDevolucion")]
        public DateTime? FechaDevolucion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("items")]
        public List<PrestamoItem>? Items { get; set; }
    }
}
