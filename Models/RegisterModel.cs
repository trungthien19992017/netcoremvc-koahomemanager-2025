using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KOAHome.Models
{
  public class RegisterModel
  {
    public int? UnitId { get; set; }
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string? PhoneNumber { get; set; }

    [Required]
    public string FullName { get; set; } // 👈 thêm dòng này

    [Required]
    [MinLength(4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [DataType(DataType.Password)]
    public string AdminPassword { get; set; }

    public string? AvatarImgUrl { get; set; } = "/img/icons/koa/SoftNhaThienLogo.png";
    public string? Position { get; set; } // Vị trí công việc của người dùng
    public int? SiteId { get; set; } // ID của site nếu có
    public string? SiteName { get; set; } // Tên của site nếu có
    public bool IsActive { get; set; } = true; // Trạng thái hoạt động của người dùng
    public bool IsDeleted { get; set; } = false; // Trạng thái đã xóa của người dùng
    public DateTime CreationTime { get; set; } = DateTime.UtcNow; // Thời gian tạo người dùng
    public int CreatorUserId { get; set; } = 0; // ID của người tạo người dùng, mặc định là 0 (admin hệ thống)
    // ✅ Danh sách role user chọn
    public List<string> SelectedRoles { get; set; } = new List<string>();

    // ✅ Danh sách role để hiển thị trong form
    public List<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();
    // ✅ Danh sách site để hiển thị trong form
    public List<SelectListItem> AvailableSites { get; set; } = new List<SelectListItem>();
  }
}
