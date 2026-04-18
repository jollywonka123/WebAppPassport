using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Configurations;

public class PassportCountryVisaConfiguration: IEntityTypeConfiguration<PassportCountryVisa>
{
    public void Configure(EntityTypeBuilder<PassportCountryVisa> builder)
    {
        builder.HasKey(x => x.Id);
    }
}