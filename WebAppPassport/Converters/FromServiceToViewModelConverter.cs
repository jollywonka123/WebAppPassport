using WebAppPassport.Common;
using WebAppPassport.Controllers.Models;
using ServiceModels = WebAppPassport.Services.Models;

namespace WebAppPassport.Converters;

public static class FromServiceToViewModelConverter
{
    // ---- Country ----

    public static CountrySummaryViewModel ToSummaryViewModel(this ServiceModels.Country c) =>
        new() { Name = c.Name, IsoShortCode = c.IsoShortCode };

    public static CountryListItemViewModel ToListItemViewModel(this ServiceModels.Country c) =>
        new() { Name = c.Name, IsoShortCode = c.IsoShortCode, Link = $"/country/{c.IsoShortCode}" };

    public static CountryDetailViewModel ToDetailViewModel(this ServiceModels.Country c)
    {
        var destinations = EmptyDestinations();
        var seen = EmptySeen();

        foreach (var dest in c.Destinations ?? [])
        {
            var label = VisaTypeLabels.From(dest.VisaType);
            foreach (var territory in dest.Passport.Countries ?? [])
            {
                if (seen[label].Add(territory.IsoShortCode))
                    destinations[label].Add(territory.ToSummaryViewModel());
            }
        }

        return new CountryDetailViewModel
        {
            Name = c.Name,
            IsoShortCode = c.IsoShortCode,
            Population = c.Population,
            Area = c.Area,
            DualCitizenshipAllowed = c.DualCitizenshipAllowed,
            Destinations = destinations
        };
    }

    // ---- Passport ----

    public static PassportListItemViewModel ToListItemViewModel(this ServiceModels.Passport p) =>
        new() { Name = p.Name, IsoShortCode = p.IsoShortCode, Link = $"/passport/{p.IsoShortCode}" };

    public static PassportRankInfoViewModel ToRankInfoViewModel(this ServiceModels.Passport p) =>
        new()
        {
            IsoShortCode = p.IsoShortCode,
            MobilityScore = p.MobilityScore,
            WorldRank = p.WorldRank,
            Link = $"/passport/{p.IsoShortCode}"
        };

    public static PassportDetailViewModel ToDetailViewModel(this ServiceModels.Passport p)
    {
        var destinations = EmptyDestinations();

        foreach (var dest in p.Destinations ?? [])
            destinations[VisaTypeLabels.From(dest.VisaType)].Add(dest.Country.ToSummaryViewModel());

        return new PassportDetailViewModel
        {
            Name = p.Name,
            IsoShortCode = p.IsoShortCode,
            MobilityScore = p.MobilityScore,
            WorldRank = p.WorldRank,
            VisaFreeCount = p.VisaFreeCount,
            VisaOnArrivalCount = p.VisaOnArrivalCount,
            EtaCount = p.EtaCount,
            RequiredVisaCount = p.RequiredVisaCount,
            TotalPopulation = p.TotalPopulation,
            Countries = (p.Countries ?? []).Select(ToSummaryViewModel).ToList(),
            Destinations = destinations
        };
    }

    // ---- Rank ----

    public static RankViewModel ToRankViewModel(
        ICollection<ServiceModels.Passport> passports,
        ICollection<ServiceModels.Country> countries)
    {
        return new RankViewModel
        {
            Passports = passports.Select(p => new RankPassportViewModel
            {
                Name = p.Name,
                IsoShortCode = p.IsoShortCode,
                MobilityScore = p.MobilityScore,
                WorldRank = p.WorldRank
            }).ToList(),
            Countries = countries.Select(c => new RankCountryViewModel
            {
                Name = c.Name,
                IsoShortCode = c.IsoShortCode,
                MobilityScore = c.Passport?.MobilityScore ?? 0,
                WorldRank = c.Passport?.WorldRank ?? 0
            }).ToList()
        };
    }

    // ---- Stack ----

    public static Dictionary<string, List<CountrySummaryViewModel>> ToStackViewModel(
        this Dictionary<VisaType, List<ServiceModels.Country>> stack)
    {
        return stack.ToDictionary(
            kvp => VisaTypeLabels.From(kvp.Key),
            kvp => kvp.Value.Select(ToSummaryViewModel).ToList()
        );
    }

    // ---- Profile ----

    public static ProfileViewModel ToProfileViewModel(this ServiceModels.User user)
    {
        return new ProfileViewModel
        {
            Username = user.Username,
            Passports = user.ShowPassports
                ? (user.Passports ?? []).Select(p => p.ToListItemViewModel()).ToList()
                : null,
            PassportCount = user.ShowPassports ? user.Passports?.Count : null,
            Countries = user.ShowCountries
                ? (user.Countries ?? []).Select(ToSummaryViewModel).ToList()
                : null,
            CountryCount = user.ShowCountries ? user.Countries?.Count : null
        };
    }

    // ---- Helpers ----

    private static Dictionary<string, List<CountrySummaryViewModel>> EmptyDestinations() => new()
    {
        [VisaTypeLabels.VisaFree] = new(),
        [VisaTypeLabels.VisaOnArrival] = new(),
        [VisaTypeLabels.ETA] = new(),
        [VisaTypeLabels.VisaRequired] = new()
    };

    private static Dictionary<string, HashSet<string>> EmptySeen() => new()
    {
        [VisaTypeLabels.VisaFree] = new(),
        [VisaTypeLabels.VisaOnArrival] = new(),
        [VisaTypeLabels.ETA] = new(),
        [VisaTypeLabels.VisaRequired] = new()
    };
}
