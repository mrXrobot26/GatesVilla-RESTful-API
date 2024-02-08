using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatesVillaAPI.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class addVillaNumberTBL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "villaNumbers",
                columns: table => new
                {
                    VillaNum = table.Column<int>(type: "int", nullable: false),
                    SpeacialDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CraetedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_villaNumbers", x => x.VillaNum);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "villaNumbers");
        }
    }
}
