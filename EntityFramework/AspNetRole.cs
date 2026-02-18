using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class AspNetRole
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string DisplayName { get; set; }

    public string PageRedirect { get; set; }

    public int? DefaultMenuId { get; set; }

    public int? SiteId { get; set; }

    public string SiteCode { get; set; }

    public string Description { get; set; }

    public int? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
}
