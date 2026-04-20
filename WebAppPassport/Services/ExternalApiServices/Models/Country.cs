using System.Text.Json.Serialization;

namespace WebAppPassport.Services.ExternalApiServices.Models;

public class Country

{

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("country")]
    public string? Name { get; set; }

}