using System.Text.Json;
using WebAppPassport.Common;
using WebAppPassport.Services.ExternalApiServices.Models;
using WebAppPassport.Services.Models;
using Country = WebAppPassport.Services.Models.Country;


namespace WebAppPassport.Services.ExternalApiServices;

public class ExternalApiService(ILogger<ExternalApiService> logger) : IExternalApiService
{
    private readonly string _endpoint = "https://api.henleypassportindex.com/api/v3/";

    public async Task<(ICollection<Country>, ICollection<Passport>)> GetAllCountriesAndPassportsAsync()
    {
        using var client = new HttpClient();

        List<Country> listCountry = new();
        List<Passport> listPassport = new();

        try
        {
            HttpResponseMessage response = await client.GetAsync(_endpoint + "countries");
            response.EnsureSuccessStatusCode();
            var countries = JsonSerializer.Deserialize<Countries>(response.Content.ReadAsStringAsync().Result);
            if (countries?.Collection == null)
                throw new NullReferenceException("After json deserialization model is null");
            foreach (var country in countries.Collection)
            {
                listPassport.Add(new Passport
                {
                    Name = country.Name!,
                    IsoShortCode = country.Code!
                });

                listCountry.Add(new Country
                {
                    Name = country.Name!,
                    IsoShortCode = country.Code!
                });
            }
        }

        catch (Exception ex) when (
            ex is HttpRequestException or OperationCanceledException or UriFormatException or ArgumentNullException
                or JsonException or NotSupportedException or NullReferenceException)
        {
            logger.LogError(ex, ex.Message);
        }

        logger.LogInformation(
            $"Retrieved {listCountry.Count} countries and {listPassport.Count} passports after successful api call");
        return (listCountry, listPassport);
    }

    public async Task<ICollection<PassportCountryVisa>> GetAllDestinationsByPassportAsync(Passport passport)
    {
        if (passport == null) throw new ArgumentNullException(nameof(passport));

        using var client = new HttpClient();

        List<PassportCountryVisa> listDestination = new();

        try
        {
            HttpResponseMessage response = await client.GetAsync(_endpoint + $"visa-single/{passport.IsoShortCode}");
            response.EnsureSuccessStatusCode();
            var countryFrom = JsonSerializer.Deserialize<CountryFrom>(response.Content.ReadAsStringAsync().Result);
            if (countryFrom?.Code == null) throw new NullReferenceException("After json deserialization model is null");
            ProcessCountryFrom(countryFrom.CollectionEta!, VisaType.ETA, ref listDestination, passport);
            ProcessCountryFrom(countryFrom.CollectionFree!, VisaType.VisaFree, ref listDestination, passport);
            ProcessCountryFrom(countryFrom.CollectionOnArrival!, VisaType.VisaOnArrival, ref listDestination, passport);
            ProcessCountryFrom(countryFrom.CollectionOnline!, VisaType.RequiredVisa, ref listDestination, passport);
            ProcessCountryFrom(countryFrom.CollectionRequired!, VisaType.RequiredVisa, ref listDestination, passport);
        }

        catch (Exception ex) when (
            ex is HttpRequestException or OperationCanceledException or UriFormatException or ArgumentNullException
                or JsonException or NotSupportedException or NullReferenceException)
        {
            logger.LogError(ex, ex.Message);
        }

        return listDestination;
    }

    private void ProcessCountryFrom(ICollection<CountryTo> collection, VisaType visaType, ref List<PassportCountryVisa> listDestination, Passport passport)
    {
        foreach (var countryTo in collection)
        {
            listDestination.Add(new PassportCountryVisa
            {
                Passport = passport,
                Country = new Country
                {
                    Name = countryTo.Name!,
                    IsoShortCode = countryTo.Code!
                },
                VisaType = visaType
            });
        }
    }

}