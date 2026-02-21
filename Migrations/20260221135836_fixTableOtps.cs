using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace diggie_server.Migrations
{
    /// <inheritdoc />
    public partial class fixTableOtps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Otps",
                type: "TEXT",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Otps",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Otps");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Otps",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Pending");
        }
    }
}
