using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migr10_RenameEVisaCountToEtaCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EVisaCount",
                table: "Passports",
                newName: "EtaCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EtaCount",
                table: "Passports",
                newName: "EVisaCount");
        }
    }
}
