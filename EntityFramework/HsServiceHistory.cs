using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsServiceHistory
{
    public int Id { get; set; }

    public string? TableName { get; set; }

    public int? ObjectId { get; set; }

    public string? Information { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
