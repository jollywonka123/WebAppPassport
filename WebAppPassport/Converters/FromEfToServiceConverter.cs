using EfEntity = WebAppPassport.DataBase.Models;
using ServiceEntity = WebAppPassport.Services.Models;

namespace WebAppPassport.Converters;

public static class FromEfToServiceConverter
{
    public static ServiceEntity.Country ToServiceEntity(this EfEntity.Country entity)
    {
        return new ServiceEntity.Country
        {
            Name = entity.Name,
            IsoShortCode =  entity.IsoShortCode,
            Population = entity.Population,
            DualCitizenshipAllowed = entity.DualCitizenshipAllowed,
            PassportValidityRequirementInSeconds = entity.PassportValidityRequirementInSeconds,
            Area = entity.Area,
        };
    }

    public static ServiceEntity.Passport ToServiceEntity(this EfEntity.Passport entity)
    {
        return new ServiceEntity.Passport
        {
            Name = entity.Name,
            IsoShortCode = entity.IsoShortCode,
        };
    }

    public static ServiceEntity.PassportCountryVisa ToServiceEntity(this EfEntity.PassportCountryVisa entity)
    {
        return new ServiceEntity.PassportCountryVisa
        {
            Passport = entity.Passport.ToServiceEntity(),
            Country = entity.Country.ToServiceEntity(),
            VisaType =  entity.VisaType,
        };
    }

    public static ServiceEntity.User ToServiceEntity(this EfEntity.User entity)
    {
        return new ServiceEntity.User
        {
            Username =  entity.Username,
            HashedPassword = entity.HashedPassword,
            MotherlandIso =  entity.MotherlandIso,
        };
    }
}