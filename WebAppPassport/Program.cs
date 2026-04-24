using System.Text;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebAppPassport.DataBase;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.BackgroundServices;
using WebAppPassport.Services.CountryService;
using WebAppPassport.Services.ExternalApiServices;
using WebAppPassport.Services.PassportService;
using WebAppPassport.Services.RankService;
using WebAppPassport.Services.StaticDataServices;
using WebAppPassport.Services.UserService;

var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Secrets", ".env");
if (File.Exists(envPath))
    Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5097";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddDatabase();
builder.Services.AddControllers();

builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddScoped<IStaticService, StaticService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IPassportService, PassportService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IRankService, RankService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHostedService<BaseSyncService>();

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new InvalidOperationException("JWT_SECRET is not set in environment");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// OpenAPI document generation (built-in .NET 10)
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "WebAppPassport API";
        document.Info.Version = "v1";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Apply pending EF migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WebAppPassport.DataBase.AppContext>();
    db.Database.Migrate();
}

// OpenAPI spec at /openapi/v1.json
app.MapOpenApi();

// Scalar UI at /scalar/v1  (supports Bearer auth out of the box)
app.MapScalarApiReference(options =>
{
    options.Title = "WebAppPassport API";
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
