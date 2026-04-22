using WebAppPassport.Common;
using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.CountryService;

public class CountryService(IRepository repository) : ICountryService
{
    public async Task<ICollection<CountryListItem>> GetAllCountriesAsync()
    {
        var efCountries = await repository.GetAllCountriesAsync();
        return (efCountries ?? [])
            .ToServiceEntity()
            .Select(c => new CountryListItem
            {
                Name = c.Name,
                IsoShortCode = c.IsoShortCode,
                Link = $"/country/{c.IsoShortCode}"
            })
            .ToList();
    }

    public async Task<CountryDetail?> GetCountryDetailAsync(string isoShortCode)
    {
        var efCountry = await repository.GetCountryWithDestinationsAsync(isoShortCode);
        if (efCountry == null) return null;

        var country = efCountry.ToServiceEntity();

        var destinations = new Dictionary<string, List<CountrySummary>>
        {
            [VisaTypeLabels.VisaFree] = new(),
            [VisaTypeLabels.VisaOnArrival] = new(),
            [VisaTypeLabels.EVisa] = new(),
            [VisaTypeLabels.VisaRequired] = new()
        };

        var addedIsos = new Dictionary<string, HashSet<string>>
        {
            [VisaTypeLabels.VisaFree] = new(),
            [VisaTypeLabels.VisaOnArrival] = new(),
            [VisaTypeLabels.EVisa] = new(),
            [VisaTypeLabels.VisaRequired] = new()
        };

        // For each destination (passport → this country), show the territories of that passport.
        // Passport.Countries is populated via PcvWithPassportCountries in the converter.
        foreach (var dest in country.Destinations ?? [])
        {
            var label = VisaTypeLabels.From(dest.VisaType);
            foreach (var territory in dest.Passport.Countries ?? [])
            {
                if (addedIsos[label].Add(territory.IsoShortCode))
                {
                    destinations[label].Add(new CountrySummary
                    {
                        Name = territory.Name,
                        IsoShortCode = territory.IsoShortCode
                    });
                }
            }
        }

        return new CountryDetail
        {
            Name = country.Name,
            IsoShortCode = country.IsoShortCode,
            Population = country.Population,
            Area = country.Area,
            DualCitizenshipAllowed = country.DualCitizenshipAllowed,
            Destinations = destinations
        };
    }
}
