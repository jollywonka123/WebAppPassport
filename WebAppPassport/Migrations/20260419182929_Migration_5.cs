using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migration_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passports_IsoShortCode",
                table: "Passports",
                column: "IsoShortCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoShortCode",
                table: "Countries",
                column: "IsoShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Passports_IsoShortCode",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Countries_IsoShortCode",
                table: "Countries");
        }
    }
}
