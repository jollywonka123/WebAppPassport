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

            // "No Reset On Close" prevents Npgsql from sending DEALLOCATE ALL on connection
            // return to pool, which can corrupt protocol state after migrations.
            // "Max Auto Prepare=0" disables prepared statement caching to avoid
            // "unexpected BackendMessageCode" errors with certain PostgreSQL versions.
            var builder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
            {
                NoResetOnClose = true,
                MaxAutoPrepare = 0
            };

            o.UseNpgsql(builder.ConnectionString);
        });
    }
}