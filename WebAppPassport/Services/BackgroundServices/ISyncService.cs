namespace WebAppPassport.Services.BackgroundServices;

public interface ISyncService
{
    public Task StartInfiniteSyncAsync();
    
    protected Task SyncIterAsync();
    
    protected DateTime? GetLastSyncTimeAsync();

    public void SetCooldownTime(int hours);
}