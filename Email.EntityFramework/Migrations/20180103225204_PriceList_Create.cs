using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Email.EntityFramework.Migrations
{
    public partial class PriceList_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastUpdate = table.Column<DateTime>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    MaxDaysForUpdate = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PriceListExtension = table.Column<string>(nullable: true),
                    PriceListFileName = table.Column<string>(nullable: true),
                    PriceListName = table.Column<string>(nullable: true),
                    PriceListUrl = table.Column<string>(nullable: true),
                    SupplierId = table.Column<int>(nullable: false),
                    UpdateFromMail = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceList_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceList_SupplierId",
                table: "PriceList",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceList");
        }
    }
}
