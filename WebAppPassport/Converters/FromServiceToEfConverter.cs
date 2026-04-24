using EfEntity = WebAppPassport.DataBase.Models;
using ServiceEntity = WebAppPassport.Services.Models;

namespace WebAppPassport.Converters;

public static class FromServiceToEfConverter
{
    public static EfEntity.Country ToEfEntity(this ServiceEntity.Country entity)
    {
        return new EfEntity.Country
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
            Population = entity.Population,
            DualCitizenshipAllowed = entity.DualCitizenshipAllowed,
            PassportValidityRequirementInSeconds = entity.PassportValidityRequirementInSeconds,
            Area = entity.Area,
        };
    }

    public static EfEntity.Passport ToEfEntity(this ServiceEntity.Passport entity)
    {
        return new EfEntity.Passport
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
            MobilityScore = entity.MobilityScore,
            WorldRank = entity.WorldRank,
            VisaFreeCount = entity.VisaFreeCount,
            VisaOnArrivalCount = entity.VisaOnArrivalCount,
            EtaCount = entity.EtaCount,
            RequiredVisaCount = entity.RequiredVisaCount,
            TotalPopulation = entity.TotalPopulation
        };
    }

    public static EfEntity.PassportCountryVisa ToEfEntity(this ServiceEntity.PassportCountryVisa entity)
    {
        return new EfEntity.PassportCountryVisa
        {
            Passport = entity.Passport.ToEfEntity(),
            Country = entity.Country.ToEfEntity(),
            VisaType = entity.VisaType,
        };
    }

    public static EfEntity.User ToEfEntity(this ServiceEntity.User entity)
    {
        return new EfEntity.User
        {
            Username = entity.Username,
            HashedPassword = entity.HashedPassword,
            MotherlandIso = entity.MotherlandIso,
        };
    }

    public static ICollection<EfEntity.PassportCountryVisa> ToEfEntity(
        this ICollection<ServiceEntity.PassportCountryVisa> entities)
        => entities.Select(pcv => pcv.ToEfEntity()).ToList();

    public static ICollection<EfEntity.Passport> ToEfEntity(this ICollection<ServiceEntity.Passport> entities)
        => entities.Select(p => p.ToEfEntity()).ToList();

    public static ICollection<EfEntity.Country> ToEfEntity(this ICollection<ServiceEntity.Country> entities)
        => entities.Select(c => c.ToEfEntity()).ToList();
}
