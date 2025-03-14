using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsBooking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public DateTime BookingDate { get; set; }

    public double? UnitPrice { get; set; }

    public double? TotalAmount { get; set; }

    /// <summary>
    /// tien coc
    /// </summary>
    public double? Deposit { get; set; }

    public double? TotalTime { get; set; }

    public string? OtherPhoneNumber { get; set; }

    public double? DiscountPercent { get; set; }

    public double? OtherDiscountAmount { get; set; }

    public string? ReasonDiscount { get; set; }

    public bool? IsPay { get; set; }

    public string? ReasonCancel { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public virtual HsCustomer Customer { get; set; } = null!;

    public virtual ICollection<HsBookingService> HsBookingServices { get; set; } = new List<HsBookingService>();

    public virtual ICollection<HsPayment> HsPayments { get; set; } = new List<HsPayment>();

    public virtual HsRoom Room { get; set; } = null!;
}
