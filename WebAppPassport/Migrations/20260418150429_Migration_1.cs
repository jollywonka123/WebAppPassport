using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migration_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_PassportId",
                table: "Countries");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_CountryId",
                table: "Passports",
                column: "CountryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Countries_CountryId",
                table: "Passports",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Countries_CountryId",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Passports_CountryId",
                table: "Passports");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_PassportId",
                table: "Countries",
                column: "PassportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
