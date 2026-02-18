using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class AspNetUser
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public string FullName { get; set; }

    public string AvatarImgUrl { get; set; }

    public string Roles { get; set; }

    public string Position { get; set; }

    public int? SiteId { get; set; }

    public string SiteName { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public int CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> RolesNavigation { get; set; } = new List<AspNetRole>();
}
