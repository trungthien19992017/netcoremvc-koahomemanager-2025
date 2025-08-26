using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KOAHome.Models
{
  public class ApplicationRole : IdentityRole<int>
  {
    public string Code { get; set; }
    public string DisplayName { get; set; }
    public string? PageRedirect { get; set; }
    public int? DefaultMenuId { get; set; }
    public int? SiteId { get; set; }
    public string? SiteCode { get; set; }
    public string? Description { get; set; }
    public int? OrderId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreationTime { get; set; }
    public int? CreatorUserId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public int? LastModifierUserId { get; set; }

  }
}
