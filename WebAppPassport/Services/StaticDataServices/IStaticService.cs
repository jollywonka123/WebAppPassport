using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.StaticDataServices;

public interface IStaticService
{
    public void EnrichCountry(ref Country country);
}