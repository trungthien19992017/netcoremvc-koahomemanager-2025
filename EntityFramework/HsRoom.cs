using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsRoom
{
    public int RoomId { get; set; }

    public int HomestayId { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public int? Floor { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public string? Color { get; set; }

    public string? BadgeClass { get; set; }

    public virtual HsHomestay Homestay { get; set; } = null!;

    public virtual ICollection<HsBooking> HsBookings { get; set; } = new List<HsBooking>();

    public virtual ICollection<HsReview> HsReviews { get; set; } = new List<HsReview>();
}
