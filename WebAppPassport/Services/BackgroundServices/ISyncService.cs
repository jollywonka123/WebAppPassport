namespace WebAppPassport.Services.BackgroundServices;

public interface ISyncService
{
    public Task StartInfiniteSyncAsync();
    
    public Task SyncIterAsync();
    
    protected DateTime? GetLastSyncTimeAsync();

    public void SetCooldownTime(int hours);
}