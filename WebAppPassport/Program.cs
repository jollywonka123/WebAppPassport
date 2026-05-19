using System.Text;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
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
        document.Info.Description = """
            ## API индекса паспортной мобильности

            Сервис предоставляет актуальную информацию о мировом рейтинге паспортов и условиях въезда в страны мира.

            ### Возможности
            - Получение рейтинга и детальной информации по паспортам и странам
            - Сравнение нескольких паспортов в одном запросе
            - Управление личной коллекцией паспортов и посещённых стран
            - Агрегированный стек доступных направлений по всем паспортам пользователя
            - Публичные профили пользователей

            ### Аутентификация
            Защищённые эндпоинты требуют JWT-токен. Получите токен через `POST /user/login` и передавайте его в заголовке:
            ```
            Authorization: Bearer <token>
            ```
            """;

        if (document.Components is not null)
        {
            document.Components.SecuritySchemes["Bearer"] = new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Введите JWT-токен, полученный через POST /user/login"
            };
        }

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, _) =>
    {
        var isAuthorized = context.Description.ActionDescriptor.EndpointMetadata
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
            .Any();

        if (isAuthorized)
        {
            operation.Security = [new Microsoft.OpenApi.OpenApiSecurityRequirement { ["Bearer"] = [] }];
        }

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
    options.Authentication = new Scalar.AspNetCore.ScalarAuthenticationOptions
    {
        PreferredSecuritySchemes = ["Bearer"]
    };
});

var forwardedOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
};
forwardedOptions.KnownIPNetworks.Clear();
forwardedOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedOptions);
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
