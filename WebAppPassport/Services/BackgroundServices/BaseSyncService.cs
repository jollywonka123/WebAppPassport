using WebAppPassport.Common;
using WebAppPassport.Converters;
using WebAppPassport.Services.ExternalApiServices;
using WebAppPassport.Services.Models;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.StaticDataServices;

namespace WebAppPassport.Services.BackgroundServices;

public class BaseSyncService(
    ILogger<BaseSyncService> logger,
    IServiceScopeFactory scopeFactory
    ) : ISyncService, IHostedService
{
    private readonly ILogger<BaseSyncService> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

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

    private async Task InitIfEmptyAsync(IRepository repository, IExternalApiService apiService, IStaticService staticService)
    {
        var existing = await repository.GetAllPassportsAsync();
        if (existing != null && existing.Any())
            return;

        _logger.LogInformation("DB is empty — initializing countries and passports from API");

        var (countries, passports) = await apiService.GetAllCountriesAndPassportsAsync();
        if (!countries.Any() || !passports.Any())
        {
            _logger.LogWarning("API returned no data for initialization");
            return;
        }

        foreach (var country in countries)
            staticService.EnrichCountry(country);

        await repository.AddPassportCountryRangeAsync(passports.ToEfEntity(), countries.ToEfEntity());
        _logger.LogInformation($"Initialized {passports.Count} passports and {countries.Count} countries");
    }

    public async Task SyncIterAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
        var apiService = scope.ServiceProvider.GetRequiredService<IExternalApiService>();
        var staticService = scope.ServiceProvider.GetRequiredService<IStaticService>();

        await InitIfEmptyAsync(repository, apiService, staticService);

        var efPassports = await repository.GetAllPassportsAsync();
        if (efPassports == null || !efPassports.Any())
        {
            _logger.LogWarning("No passports found in DB, skipping sync");
            return;
        }

        var passports = efPassports.ToServiceEntity();
        List<PassportCountryVisa> allDestinations = new();

        foreach (var passport in passports)
        {
            var destinations = await apiService.GetAllDestinationsByPassportAsync(passport);
            if (destinations.Count == 0)
            {
                _logger.LogWarning($"No destinations returned for passport '{passport.Name}' during sync");
                continue;
            }
            allDestinations.AddRange(destinations);
        }

        await repository.UpdateDestinationsRangeAsync(allDestinations.ToEfEntity());
        _logger.LogInformation($"Updated {allDestinations.Count} destination rules");

        await ComputeAndSaveMetricsAsync(repository);
    }

    private async Task ComputeAndSaveMetricsAsync(IRepository repository)
    {
        var passports = await repository.GetAllPassportsWithCountriesAndDestinationsAsync();

        foreach (var passport in passports)
        {
            var destinations = passport.PassportCountryVisas ?? [];
            passport.VisaFreeCount = destinations.Count(d => d.VisaType == VisaType.VisaFree);
            passport.VisaOnArrivalCount = destinations.Count(d => d.VisaType == VisaType.VisaOnArrival);
            passport.EtaCount = destinations.Count(d => d.VisaType == VisaType.ETA);
            passport.RequiredVisaCount = destinations.Count(d =>
                d.VisaType == VisaType.RequiredVisa || d.VisaType == VisaType.ETA);
            passport.MobilityScore = passport.VisaFreeCount + passport.VisaOnArrivalCount;
            passport.TotalPopulation = (passport.Countries ?? []).Sum(c => c.Population ?? 0);
        }

        var ordered = passports.OrderByDescending(p => p.MobilityScore).ToList();
        for (int i = 0; i < ordered.Count; i++)
            ordered[i].WorldRank = i + 1;

        await repository.SaveChangesAsync();
        _logger.LogInformation($"Computed metrics for {passports.Count} passports");
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