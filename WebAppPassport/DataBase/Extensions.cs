using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace WebAppPassport.DataBase;

public static class Extensions
{
    public static void AddDatabase(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<AppContext>(o =>
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            
            o.UseNpgsql(connectionString);
        });
    }
}