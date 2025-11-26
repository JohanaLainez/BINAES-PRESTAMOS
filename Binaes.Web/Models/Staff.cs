using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Staff
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("rol")]
        public string? Rol { get; set; }

        [JsonPropertyName("activo")]
        public bool Activo { get; set; } = true;
    }
}
