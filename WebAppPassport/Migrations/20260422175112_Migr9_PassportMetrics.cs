using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migr9_PassportMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EVisaCount",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MobilityScore",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredVisaCount",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TotalPopulation",
                table: "Passports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaFreeCount",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VisaOnArrivalCount",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorldRank",
                table: "Passports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EVisaCount",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "MobilityScore",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "RequiredVisaCount",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "TotalPopulation",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "VisaFreeCount",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "VisaOnArrivalCount",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "WorldRank",
                table: "Passports");
        }
    }
}
