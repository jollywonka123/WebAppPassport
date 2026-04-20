using System.Text.Json.Serialization;

namespace WebAppPassport.Services.ExternalApiServices.Models;

public class Countries
{
    [JsonPropertyName("countries")]
    public List<Country>? Collection { get; set; }
}