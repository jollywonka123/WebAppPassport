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

    public static ICollection<ServiceEntity.Passport> ToServiceEntity(this ICollection<EfEntity.Passport> entities)
    {
        List<ServiceEntity.Passport> newPassports = new();
        
        foreach (var entity in entities)
        {
            newPassports.Add(entity.ToServiceEntity());
        }
        return newPassports;
    }
    
    public static ICollection<ServiceEntity.Country> ToServiceEntity(this ICollection<EfEntity.Country> entities)
    {
        List<ServiceEntity.Country> newCountries = new();
        
        foreach (var entity in entities)
        {
            newCountries.Add(entity.ToServiceEntity());
        }
        return newCountries;
    }
    
    public static ICollection<ServiceEntity.PassportCountryVisa> ToServiceEntity(
        this ICollection<EfEntity.PassportCountryVisa> entities)
    {
        List<ServiceEntity.PassportCountryVisa> newDest = new();
        
        foreach (var entity in entities)
        {
            newDest.Add(entity.ToServiceEntity());
        }
        return newDest;
    }
}