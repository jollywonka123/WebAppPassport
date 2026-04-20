using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ExternalApiServices;
using WebAppPassport.Services.Models;
using WebAppPassport.Services.StaticDataServices;

namespace WebAppPassport.Services.BackgroundServices;

public class BaseSyncService(
    ILogger<BaseSyncService> logger, 
    IExternalApiService apiService, 
    IStaticService staticService,
    IRepository repository
    ) : ISyncService, IHostedService
{
    private readonly ILogger<BaseSyncService> _logger = logger;
    
    private IRepository _repository = repository;
    
    private IExternalApiService _apiService = apiService;
    
    private IStaticService _staticService = staticService;
    
    protected DateTime? LastSyncTime;
    
    protected int CooldownHours = 24;

    public Task StartInfiniteSyncAsync()
    {
        throw new NotImplementedException();
    }

    private async Task InitDataAsync()
    {
        var countriesAndPassports = await _apiService.GetAllCountriesAndPassportsAsync();
        if (!countriesAndPassports.Item1.Any()) _logger.LogWarning("Countries and passports are empty after request");
        List<ICollection<PassportCountryVisa>> listOfDestinations = new();

        foreach (var passportItem in countriesAndPassports.Item2)
        {
            var dest = await _apiService.GetAllDestinationsByPassportAsync(passportItem);
            if (!dest.Any())
            {
                _logger.LogWarning($"Destinations for passport \'{passportItem.Name}\' not found");
                continue;
            }
            listOfDestinations.Add(dest);
        }

        for (int i = 0; i < countriesAndPassports.Item1.Count; i++)
        {
            _staticService.EnrichCountry(countriesAndPassports.Item1.ElementAt(i));
        }

        for (int i = 0; i < listOfDestinations.Count; i++)
        {
            for (int j = 0; j < listOfDestinations.ElementAt(i).Count; j++)
            {
                _staticService.EnrichCountry(listOfDestinations.ElementAt(i).ElementAt(j).Country);
            }
        }

        await _repository.AddPassportCountryRangeAsync(countriesAndPassports.Item2.ToEfEntity(),
            countriesAndPassports.Item1.ToEfEntity());

        foreach (var destOfDest in listOfDestinations)
        {
            foreach (var dest in destOfDest)
            {
                await _repository.AddDestinationAsync(dest.ToEfEntity());
            }
        }
    }

    public async Task SyncIterAsync()
    {
        await InitDataAsync();
    }

    public DateTime? GetLastSyncTimeAsync()
    {
        return LastSyncTime;
    }

    public void SetCooldownTime(int hours)
    {
        if (hours > 0)
            CooldownHours = hours;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitDataAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}