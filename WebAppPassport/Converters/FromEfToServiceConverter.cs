using EfEntity = WebAppPassport.DataBase.Models;
using ServiceEntity = WebAppPassport.Services.Models;

namespace WebAppPassport.Converters;

public static class FromEfToServiceConverter
{
    // ---- Country ----

    public static ServiceEntity.Country ToServiceEntity(this EfEntity.Country entity)
    {
        return new ServiceEntity.Country
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
            Population = entity.Population,
            DualCitizenshipAllowed = entity.DualCitizenshipAllowed,
            PassportValidityRequirementInSeconds = entity.PassportValidityRequirementInSeconds,
            Area = entity.Area,
            // Shallow passport (metrics only, no collections) — breaks Country→Passport→Countries→Country cycle
            Passport = entity.Passport != null ? PassportShallow(entity.Passport) : null,
            // PCVs for country detail: passport inside carries its territories
            Destinations = entity.PassportCountryVisas?
                .Select(PcvWithPassportCountries)
                .ToList()
        };
    }

    public static ICollection<ServiceEntity.Country> ToServiceEntity(this ICollection<EfEntity.Country> entities)
        => entities.Select(c => c.ToServiceEntity()).ToList();

    // ---- Passport ----

    public static ServiceEntity.Passport ToServiceEntity(this EfEntity.Passport entity)
    {
        return new ServiceEntity.Passport
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
            MobilityScore = entity.MobilityScore,
            WorldRank = entity.WorldRank,
            VisaFreeCount = entity.VisaFreeCount,
            VisaOnArrivalCount = entity.VisaOnArrivalCount,
            EtaCount = entity.EtaCount,
            RequiredVisaCount = entity.RequiredVisaCount,
            TotalPopulation = entity.TotalPopulation,
            // Territories as summaries only — breaks Passport→Countries→Country→Passport cycle
            Countries = entity.Countries?.Select(CountrySummaryOnly).ToList(),
            // Destinations as PCVs with shallow passport — breaks Passport→PCVs→Passport cycle
            Destinations = entity.PassportCountryVisas?.Select(PcvWithCountrySummary).ToList()
        };
    }

    public static ICollection<ServiceEntity.Passport> ToServiceEntity(this ICollection<EfEntity.Passport> entities)
        => entities.Select(p => p.ToServiceEntity()).ToList();

    // ---- PassportCountryVisa ----

    public static ServiceEntity.PassportCountryVisa ToServiceEntity(this EfEntity.PassportCountryVisa entity)
    {
        return new ServiceEntity.PassportCountryVisa
        {
            VisaType = entity.VisaType,
            Passport = entity.Passport.ToServiceEntity(),
            Country = entity.Country.ToServiceEntity()
        };
    }

    public static ICollection<ServiceEntity.PassportCountryVisa> ToServiceEntity(
        this ICollection<EfEntity.PassportCountryVisa> entities)
        => entities.Select(pcv => pcv.ToServiceEntity()).ToList();

    // ---- User ----

    public static ServiceEntity.User ToServiceEntity(this EfEntity.User entity)
    {
        return new ServiceEntity.User
        {
            Username = entity.Username,
            HashedPassword = entity.HashedPassword,
            MotherlandIso = entity.MotherlandIso,
            Passports = entity.Passports?.Select(p => p.ToServiceEntity()).ToList()
        };
    }

    // ---- Private helpers (break circular references) ----

    // Country with only Name + IsoShortCode (no Passport, no Destinations)
    private static ServiceEntity.Country CountrySummaryOnly(EfEntity.Country c) =>
        new() { Name = c.Name, IsoShortCode = c.IsoShortCode };

    // Passport with metrics only (no Countries, no Destinations)
    private static ServiceEntity.Passport PassportShallow(EfEntity.Passport p) =>
        new()
        {
            Name = p.Name,
            IsoShortCode = p.IsoShortCode,
            MobilityScore = p.MobilityScore,
            WorldRank = p.WorldRank,
            VisaFreeCount = p.VisaFreeCount,
            VisaOnArrivalCount = p.VisaOnArrivalCount,
            EtaCount = p.EtaCount,
            RequiredVisaCount = p.RequiredVisaCount,
            TotalPopulation = p.TotalPopulation
        };

    // Passport with Countries (territories as summaries, no Destinations)
    // Used inside PCVs of a Country — so CountryDetail can show which territories have access
    private static ServiceEntity.Passport PassportWithCountries(EfEntity.Passport p) =>
        new()
        {
            Name = p.Name,
            IsoShortCode = p.IsoShortCode,
            MobilityScore = p.MobilityScore,
            WorldRank = p.WorldRank,
            Countries = p.Countries?.Select(CountrySummaryOnly).ToList()
        };

    // PCV used inside Passport.Destinations: Country is minimal, Passport is shallow
    private static ServiceEntity.PassportCountryVisa PcvWithCountrySummary(EfEntity.PassportCountryVisa pcv) =>
        new()
        {
            VisaType = pcv.VisaType,
            Country = CountrySummaryOnly(pcv.Country),
            Passport = PassportShallow(pcv.Passport)
        };

    // PCV used inside Country.Destinations: Passport carries its territories list
    private static ServiceEntity.PassportCountryVisa PcvWithPassportCountries(EfEntity.PassportCountryVisa pcv) =>
        new()
        {
            VisaType = pcv.VisaType,
            Country = CountrySummaryOnly(pcv.Country),
            Passport = PassportWithCountries(pcv.Passport)
        };
}
