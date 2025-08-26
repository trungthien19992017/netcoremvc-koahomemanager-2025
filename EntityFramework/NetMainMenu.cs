using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetMainMenu
{
    public int Id { get; set; }

    public int? TenantId { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Icon { get; set; }

    public string? Description { get; set; }

    public int? Parent { get; set; }

    public string? Link { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int Index { get; set; }

    public string? RequiredPermissionName { get; set; }

    public long? CreatorUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public long? LastModifierUserId { get; set; }

    public int? DeviceType { get; set; }

    public bool? IsMobile { get; set; }

    public string? Code { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public bool? IsMiniItem { get; set; }

    public string? ImageUrl { get; set; }
}
