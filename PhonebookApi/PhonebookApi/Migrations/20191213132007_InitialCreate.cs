﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PhonebookApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhonebookEntries",
                columns: table => new
                {
                    PhonebookEntryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhonebookEntries", x => x.PhonebookEntryId);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    ContactDetailId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhonebookEntryId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.ContactDetailId);
                    table.ForeignKey(
                        name: "FK_ContactDetails_PhonebookEntries_PhonebookEntryId",
                        column: x => x.PhonebookEntryId,
                        principalTable: "PhonebookEntries",
                        principalColumn: "PhonebookEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_PhonebookEntryId",
                table: "ContactDetails",
                column: "PhonebookEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "PhonebookEntries");
        }
    }
}
