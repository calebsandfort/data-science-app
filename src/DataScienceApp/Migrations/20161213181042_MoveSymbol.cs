using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataScienceApp.Migrations
{
    public partial class MoveSymbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "PriceEntries");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Stocks",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "PriceEntries",
                maxLength: 4,
                nullable: true);
        }
    }
}
