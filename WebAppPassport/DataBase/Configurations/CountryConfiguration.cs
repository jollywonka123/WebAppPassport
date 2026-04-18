using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Configurations;

public class CountryConfiguration: IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Passport)
            .WithOne(x => x.Country)
            .HasForeignKey<Passport>(x => x.CountryId);

        builder.HasMany(x => x.PassportCountryVisas)
            .WithOne(x => x.Country)
            .HasForeignKey(x => x.ToCountryId);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Countries);
    }
}