using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Configurations;

public class PassportConfiguration: IEntityTypeConfiguration<Passport>
{
    public void Configure(EntityTypeBuilder<Passport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.PassportCountryVisas)
            .WithOne(x => x.Passport)
            .HasForeignKey(x => x.UsingPassportId);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Passports);
    }
}