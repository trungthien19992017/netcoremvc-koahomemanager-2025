using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetTenant
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string? Description { get; set; }

    public int? StartNumberProd { get; set; }

    public string? TenantLogoUrl { get; set; }

    public string? TenantIcoUrl { get; set; }

    public string? TenantLogoTextUrl { get; set; }

    public int? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
