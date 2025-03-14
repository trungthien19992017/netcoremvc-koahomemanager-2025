using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsPayment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public double? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentInformation { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual HsBooking Booking { get; set; } = null!;
}
