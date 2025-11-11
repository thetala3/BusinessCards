using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessCards.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_DateOfBirth",
                table: "BusinessCards",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_Email",
                table: "BusinessCards",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_Gender",
                table: "BusinessCards",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_Name",
                table: "BusinessCards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCards_Phone",
                table: "BusinessCards",
                column: "Phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessCards");
        }
    }
}
