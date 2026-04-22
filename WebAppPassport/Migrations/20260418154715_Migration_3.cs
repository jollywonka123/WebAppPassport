using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migration_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_VisaRules_VisaRuleId",
                table: "Destinations");

            migrationBuilder.DropTable(
                name: "UserCountries");

            migrationBuilder.DropTable(
                name: "UserPassports");

            migrationBuilder.DropTable(
                name: "VisaRules");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_VisaRuleId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "VisaRegimeId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "VisaRuleId",
                table: "Destinations");

            migrationBuilder.AddColumn<int>(
                name: "VisaType",
                table: "Destinations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CountryUser",
                columns: table => new
                {
                    CountriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryUser", x => new { x.CountriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_CountryUser_Countries_CountriesId",
                        column: x => x.CountriesId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CountryUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassportUser",
                columns: table => new
                {
                    PassportsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassportUser", x => new { x.PassportsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_PassportUser_Passports_PassportsId",
                        column: x => x.PassportsId,
                        principalTable: "Passports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassportUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryUser_UsersId",
                table: "CountryUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PassportUser_UsersId",
                table: "PassportUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryUser");

            migrationBuilder.DropTable(
                name: "PassportUser");

            migrationBuilder.DropColumn(
                name: "VisaType",
                table: "Destinations");

            migrationBuilder.AddColumn<Guid>(
                name: "VisaRegimeId",
                table: "Destinations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "VisaRuleId",
                table: "Destinations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCountries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCountries_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCountries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPassports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassportId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPassports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPassports_Passports_PassportId",
                        column: x => x.PassportId,
                        principalTable: "Passports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPassports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisaRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaRules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_VisaRuleId",
                table: "Destinations",
                column: "VisaRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCountries_CountryId",
                table: "UserCountries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCountries_UserId",
                table: "UserCountries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassports_PassportId",
                table: "UserPassports",
                column: "PassportId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPassports_UserId",
                table: "UserPassports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_VisaRules_VisaRuleId",
                table: "Destinations",
                column: "VisaRuleId",
                principalTable: "VisaRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
