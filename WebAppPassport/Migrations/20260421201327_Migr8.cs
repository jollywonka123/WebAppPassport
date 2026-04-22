using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppPassport.Migrations
{
    /// <inheritdoc />
    public partial class Migr8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassportId",
                table: "Countries",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries");

            migrationBuilder.AlterColumn<Guid>(
                name: "PassportId",
                table: "Countries",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Passports_PassportId",
                table: "Countries",
                column: "PassportId",
                principalTable: "Passports",
                principalColumn: "Id");
        }
    }
}
