using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataScienceApp.Migrations
{
    public partial class AddPctChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PctChange",
                table: "PriceEntries",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "YdaPctChange",
                table: "PriceEntries",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PctChange",
                table: "PriceEntries");

            migrationBuilder.DropColumn(
                name: "YdaPctChange",
                table: "PriceEntries");
        }
    }
}
