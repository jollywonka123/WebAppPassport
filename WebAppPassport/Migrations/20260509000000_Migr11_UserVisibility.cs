using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migr11_UserVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowCountries",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowPassports",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowCountries",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShowPassports",
                table: "Users");
        }
    }
}
