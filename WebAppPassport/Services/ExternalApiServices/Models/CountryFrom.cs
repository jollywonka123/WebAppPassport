using System.Text.Json.Serialization;

namespace WebAppPassport.Services.ExternalApiServices.Models;

public class CountryFrom
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("country")]
    public string? Name { get; set; }
    
    [JsonPropertyName("visa_on_arrival")]
    public ICollection<CountryTo>? CollectionOnArrival { get; set; } = new List<CountryTo>();
    
    [JsonPropertyName("visa_required")]
    public ICollection<CountryTo>? CollectionRequired { get; set; } = new List<CountryTo>();
    
    [JsonPropertyName("visa_online")]
    public ICollection<CountryTo>? CollectionOnline { get; set; } = new List<CountryTo>();
    
    [JsonPropertyName("electronic_travel_authorisation")]
    public ICollection<CountryTo>? CollectionEta { get; set; } = new List<CountryTo>();

    [JsonPropertyName("visa_free_access")] 
    public ICollection<CountryTo>? CollectionFree { get; set; } = new List<CountryTo>();
}