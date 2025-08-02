using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KOAHome.Migrations
{
    /// <inheritdoc />
    public partial class _20250727_RemoveIdentityTables : Migration
    {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      // ✅ Xóa theo thứ tự tránh conflict FK
      migrationBuilder.DropTable(name: "AspNetUserTokens");
      migrationBuilder.DropTable(name: "AspNetUserLogins");
      migrationBuilder.DropTable(name: "AspNetUserRoles");
      migrationBuilder.DropTable(name: "AspNetUserClaims");
      migrationBuilder.DropTable(name: "AspNetRoleClaims");
      migrationBuilder.DropTable(name: "AspNetUsers");
      migrationBuilder.DropTable(name: "AspNetRoles");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      // ✅ Down() có thể để trống hoặc tạo lại bảng (nếu cần rollback)
      // Ở đây để trống vì bạn sẽ tạo lại Identity với int PK trong migration kế tiếp
    }
  }
}
