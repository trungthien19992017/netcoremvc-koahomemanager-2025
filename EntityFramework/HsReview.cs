using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsReview
{
    public int ReviewId { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual HsCustomer Customer { get; set; } = null!;

    public virtual HsRoom Room { get; set; } = null!;
}
