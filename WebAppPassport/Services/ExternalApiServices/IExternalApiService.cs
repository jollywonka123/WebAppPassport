namespace WebAppPassport.Services.ExternalApiServices;

public interface IExternalApiService
{
    public Task GetAllCountriesAsync();
    
}

/*
 * с помощью Henly можно получить след данные
 * для Country: Name, Iso, | DualCitizenship / Population, PassportValidity requirement
 * для Passport: Name, Iso,
 * для PassportCountryVisa: VisaType
 */