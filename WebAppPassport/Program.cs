using DotNetEnv;
using WebAppPassport.DataBase;

var envPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Secrets", ".env");
Env.Load(envPath);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabase();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();