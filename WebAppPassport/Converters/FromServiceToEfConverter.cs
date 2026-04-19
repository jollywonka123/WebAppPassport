using EfEntity = WebAppPassport.DataBase.Models;
using ServiceEntity = WebAppPassport.Services.ServiceModels;

namespace WebAppPassport.Converters;

public static class FromServiceToEfConverter
{
    public static EfEntity.Country ToEfEntity(this ServiceEntity.Country entity)
    {
        return new EfEntity.Country
        {
            Name = entity.Name,
            IsoShortCode =  entity.IsoShortCode,
            Population = entity.Population,
            DualCitizenshipAllowed = entity.DualCitizenshipAllowed,
            PassportValidityRequirementInSeconds = entity.PassportValidityRequirementInSeconds,
        };
    }

    public static EfEntity.Passport ToEfEntity(this ServiceEntity.Passport entity)
    {
        return new EfEntity.Passport
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
        };
    }

    public static EfEntity.PassportCountryVisa ToEfEntity(this ServiceEntity.PassportCountryVisa entity)
    {
        return new EfEntity.PassportCountryVisa
        {
            Passport = entity.Passport.ToEfEntity(),
            Country = entity.Country.ToEfEntity(),
            VisaType =  entity.VisaType,
        };
    }

    public static EfEntity.User ToEfEntity(this ServiceEntity.User entity)
    {
        return new EfEntity.User
        {
            Username =  entity.Username,
            HashedPassword = entity.HashedPassword,
            MotherlandIso =  entity.MotherlandIso,
        };
    }
}