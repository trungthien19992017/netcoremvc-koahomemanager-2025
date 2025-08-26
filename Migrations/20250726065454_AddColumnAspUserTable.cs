using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KOAHome.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnAspUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarImgUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);


            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);


            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
      migrationBuilder.AddColumn<DateTime>(
          name: "CreationTime",
          table: "AspNetUsers",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

      migrationBuilder.AddColumn<int>(
          name: "CreatorUserId",
          table: "AspNetUsers",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<DateTime>(
          name: "LastModificationTime",
          table: "AspNetUsers",
          type: "datetime2",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "LastModifierUserId",
          table: "AspNetUsers",
          type: "int",
          nullable: true);
    }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImgUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "AspNetUsers");
        }
    }
}
