using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class CategoryDetail
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryCode { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? OrderId { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
