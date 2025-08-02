using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KOAHome.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomFieldsToRole_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

      migrationBuilder.AddColumn<string>(
          name: "Code",
          table: "AspNetRoles",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "DisplayName",
          table: "AspNetRoles",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "PageRedirect",
          table: "AspNetRoles",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "DefaultMenuId",
          table: "AspNetRoles",
          type: "int",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "SiteCode",
          table: "AspNetRoles",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "SiteId",
          table: "AspNetRoles",
          type: "int",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "Description",
          table: "AspNetRoles",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "OrderId",
          table: "AspNetRoles",
          type: "int",
          nullable: true);

      migrationBuilder.AddColumn<bool>(
          name: "IsActive",
          table: "AspNetRoles",
          type: "bit",
          nullable: true);

      migrationBuilder.AddColumn<bool>(
          name: "IsDeleted",
          table: "AspNetRoles",
          type: "bit",
          nullable: true);

      migrationBuilder.AddColumn<DateTime>(
          name: "CreationTime",
          table: "AspNetRoles",
          type: "datetime2",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "CreatorUserId",
          table: "AspNetRoles",
          type: "int",
          nullable: true);

      migrationBuilder.AddColumn<DateTime>(
          name: "LastModificationTime",
          table: "AspNetRoles",
          type: "datetime2",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "LastModifierUserId",
          table: "AspNetRoles",
          type: "int",
          nullable: true);
    }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
