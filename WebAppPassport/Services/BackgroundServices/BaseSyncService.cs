using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ExternalApiServices;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.BackgroundServices;

public class BaseSyncService(
    ILogger<BaseSyncService> logger,
    IExternalApiService apiService,
    IRepository repository
    ) : ISyncService, IHostedService
{
    private readonly ILogger<BaseSyncService> _logger = logger;

    private IRepository _repository = repository;

    private IExternalApiService _apiService = apiService;

    protected DateTime? LastSyncTime;
    
    protected int CooldownHours = 24;

    public async Task StartInfiniteSyncAsync()
    {
        while (true)
        {
            try
            {
                await SyncIterAsync();
                LastSyncTime = DateTime.UtcNow;
                _logger.LogInformation($"Sync completed at {LastSyncTime}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sync iteration");
            }

            await Task.Delay(TimeSpan.FromHours(CooldownHours));
        }
    }

    public async Task SyncIterAsync()
    {
        var efPassports = await _repository.GetAllPassportsAsync();
        if (efPassports == null || !efPassports.Any())
        {
            _logger.LogWarning("No passports found in DB, skipping sync");
            return;
        }

        var passports = efPassports.ToServiceEntity();
        List<PassportCountryVisa> allDestinations = new();

        foreach (var passport in passports)
        {
            var destinations = await _apiService.GetAllDestinationsByPassportAsync(passport);
            if (destinations.Count == 0)
            {
                _logger.LogWarning($"No destinations returned for passport '{passport.Name}' during sync");
                continue;
            }
            allDestinations.AddRange(destinations);
        }

        await _repository.UpdateDestinationsRangeAsync(allDestinations.ToEfEntity());
        _logger.LogInformation($"Updated {allDestinations.Count} destination rules");
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = StartInfiniteSyncAsync();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}