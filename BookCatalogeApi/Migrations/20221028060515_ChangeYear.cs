using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCatalogeApi.Migrations
{
    public partial class ChangeYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Books",
                type: "integer",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Cover",
                table: "Books",
                type: "bytea",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Year",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Books",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}
