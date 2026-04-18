using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Configurations;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase;

public class AppContext(DbContextOptions<AppContext> options): DbContext(options)
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<PassportCountryVisa> Destinations { get; set; }
    public DbSet<Passport> Passports { get; set; }
    public DbSet<User> Users { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new PassportConfiguration());
        modelBuilder.ApplyConfiguration(new PassportCountryVisaConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}