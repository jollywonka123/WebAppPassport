using System.Text.Json.Serialization;

namespace WebAppPassport.Services.ExternalApiServices.Models;

public class CountryTo
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}