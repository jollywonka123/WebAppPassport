namespace WebAppPassport.Services.BackgroundServices;

public class BaseSyncService(ILogger<BaseSyncService> logger) : ISyncService
{
    private readonly ILogger<BaseSyncService> _logger = logger;
    protected DateTime? LastSyncTime;
    protected int CooldownHours = 24;

    public Task StartInfiniteSyncAsync()
    {
        throw new NotImplementedException();
    }

    public Task SyncIterAsync()
    {
        throw new NotImplementedException();
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
}