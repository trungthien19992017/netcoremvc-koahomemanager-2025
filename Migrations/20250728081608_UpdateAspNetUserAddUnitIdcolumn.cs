using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KOAHome.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAspNetUserAddUnitIdcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "AspNetUsers");
        }
    }
}
