using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsBookingService
{
    public int BookingServiceId { get; set; }

    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public int? Quantity { get; set; }

    public double? TotalPrice { get; set; }

    public DateTime? AdditionFromDate { get; set; }

    public DateTime? AdditionToDate { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual HsBooking Booking { get; set; } = null!;

    public virtual HsService Service { get; set; } = null!;
}
