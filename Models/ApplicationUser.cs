using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOAHome.Models
{
  public class ApplicationUser : IdentityUser<int>
  {
    public int UnitId { get; set; }
    public string FullName { get; set; }
    public string? AvatarImgUrl { get; set; } = "/img/icons/koa/SoftNhaThienLogo.png";
    public string? Roles { get; set; } // Lưu danh sách role dưới cắt chuỗi
    public string? Position { get; set; } // Vị trí công việc của người dùng
    public int? SiteId { get; set; } // ID của site nếu có
    public string? SiteName { get; set; } // Tên của site nếu có
    public bool IsActive { get; set; } = true; // Trạng thái hoạt động của người dùng
    public bool IsDeleted { get; set; } = false; // Trạng thái đã xóa của người dùng
    public DateTime CreationTime { get; set; } = DateTime.UtcNow; // Thời gian tạo người dùng
    public int CreatorUserId { get; set; } = 0; // ID của người tạo người dùng, mặc định là 0 (admin hệ thống)
    public DateTime? LastModificationTime { get; set; } // Thời gian đăng nhập lần cuối
    public int? LastModifierUserId { get; set; } // ID của người sửa đổi lần cuối
  }
}
