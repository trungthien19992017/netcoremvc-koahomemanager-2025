using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetUnit
{
    public long Id { get; set; }

    public string? ShortName { get; set; }

    public string? UnitCode { get; set; }

    public string? UnitName { get; set; }

    public int? UnitGroup { get; set; }

    public long? ParentId { get; set; }

    public int? OrderId { get; set; }

    public int? LeadUserId { get; set; }

    public string? UnitType { get; set; }

    public string? Description { get; set; }

    public bool? IsParent { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public long? RootId { get; set; }
}
