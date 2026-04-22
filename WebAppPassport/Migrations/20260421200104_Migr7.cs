using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migr7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Countries_CountryId",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Passports_CountryId",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Passports");

            migrationBuilder.AddColumn<Guid>(
                name: "PassportId",
                table: "Countries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_PassportId",
                table: "Countries",
                column: "PassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id");
            
            migrationBuilder.Sql(@"
                UPDATE ""Countries"" c
                SET ""PassportId"" = p.""Id""
                FROM ""Passports"" p
                WHERE c.""IsoShortCode"" = p.""IsoShortCode""
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_PassportId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "PassportId",
                table: "Countries");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Passports",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
            
            migrationBuilder.Sql(@"
                UPDATE ""Passports"" p
                SET ""CountryId"" = c.""Id""
                FROM ""Countries"" c
                WHERE c.""IsoShortCode"" = p.""IsoShortCode""
            ");
        }
    }
}
