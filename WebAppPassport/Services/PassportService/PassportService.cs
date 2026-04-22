using WebAppPassport.Common;
using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.PassportService;

public class PassportService(IRepository repository) : IPassportService
{
    public async Task<ICollection<PassportListItem>> GetAllPassportsAsync()
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        return efPassports
            .ToServiceEntity()
            .Select(p => new PassportListItem
            {
                Name = p.Name,
                IsoShortCode = p.IsoShortCode,
                Link = $"/passport/{p.IsoShortCode}"
            })
            .ToList();
    }

    public async Task<PassportDetail?> GetPassportDetailAsync(string isoShortCode)
    {
        var efPassport = await repository.GetPassportWithDestinationsAsync(isoShortCode);
        if (efPassport == null) return null;

        var passport = efPassport.ToServiceEntity();

        var destinations = new Dictionary<string, List<CountrySummary>>
        {
            [VisaTypeLabels.VisaFree] = new(),
            [VisaTypeLabels.VisaOnArrival] = new(),
            [VisaTypeLabels.EVisa] = new(),
            [VisaTypeLabels.VisaRequired] = new()
        };

        foreach (var dest in passport.Destinations ?? [])
        {
            destinations[VisaTypeLabels.From(dest.VisaType)].Add(new CountrySummary
            {
                Name = dest.Country.Name,
                IsoShortCode = dest.Country.IsoShortCode
            });
        }

        return new PassportDetail
        {
            Name = passport.Name,
            IsoShortCode = passport.IsoShortCode,
            MobilityScore = passport.MobilityScore,
            WorldRank = passport.WorldRank,
            VisaFreeCount = passport.VisaFreeCount,
            VisaOnArrivalCount = passport.VisaOnArrivalCount,
            EVisaCount = passport.EVisaCount,
            RequiredVisaCount = passport.RequiredVisaCount,
            TotalPopulation = passport.TotalPopulation,
            Countries = (passport.Countries ?? [])
                .Select(c => new CountrySummary { Name = c.Name, IsoShortCode = c.IsoShortCode })
                .ToList(),
            Destinations = destinations
        };
    }

    public async Task<PassportRankInfo?> GetPassportByIsoAsync(string isoShortCode)
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        var passport = efPassports
            .ToServiceEntity()
            .FirstOrDefault(p => p.IsoShortCode == isoShortCode);

        if (passport == null) return null;

        return new PassportRankInfo
        {
            IsoShortCode = passport.IsoShortCode,
            MobilityScore = passport.MobilityScore,
            WorldRank = passport.WorldRank,
            Link = $"/passport/{passport.IsoShortCode}"
        };
    }
}
