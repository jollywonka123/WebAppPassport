using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
