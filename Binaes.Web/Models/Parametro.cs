using System.Text.Json.Serialization;

namespace Binaes.Web.Models
{
    public class Parametro
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("plazoDias")]
        public int PlazoDias { get; set; }

        [JsonPropertyName("topePrestamosActivos")]
        public int TopePrestamosActivos { get; set; }

        [JsonPropertyName("maxRenovaciones")]
        public int MaxRenovaciones { get; set; }
    }
}
