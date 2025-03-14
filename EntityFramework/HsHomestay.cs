using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsHomestay
{
    public int HomestayId { get; set; }

    public int OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual ICollection<HsRoom> HsRooms { get; set; } = new List<HsRoom>();

    public virtual HsOwner Owner { get; set; } = null!;
}
