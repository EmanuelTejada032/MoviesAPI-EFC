using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAPI_EFC.Migrations
{
    public partial class customexcfilterlogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomExceptionFilterLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomExceptionFilterLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomExceptionFilterLogs");
        }
    }
}
