using DotNetEnv;
using WebAppPassport.DataBase;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.BackgroundServices;
using WebAppPassport.Services.ExternalApiServices;
using WebAppPassport.Services.StaticDataServices;

var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Secrets", ".env");
Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabase();

builder.Services.AddScoped<ISyncService, BaseSyncService>();
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddScoped<IStaticService, StaticService>();
builder.Services.AddScoped<ISyncService, BaseSyncService>();
builder.Services.AddScoped<IRepository,  Repository>();
//builder.Services.AddHostedService<BaseSyncService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var myService = scope.ServiceProvider.GetRequiredService<ISyncService>();
    await myService.SyncIterAsync();
}

app.MapGet("/", () => "Hello World!");


app.Run();