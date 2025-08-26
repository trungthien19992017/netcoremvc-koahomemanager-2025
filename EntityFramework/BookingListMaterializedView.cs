using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class BookingListMaterializedView
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public string LastName { get; set; } = null!;

    public string Mxh { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string Gender { get; set; } = null!;

    public string GenderIcon { get; set; } = null!;

    public string GenderColorClass { get; set; } = null!;

    public string? Cccd { get; set; }

    public string RoomName { get; set; } = null!;

    public string? RoomBadgeClass { get; set; }

    public string Name { get; set; } = null!;

    public int? Floor { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public DateTime BookingDate { get; set; }

    public double? UnitPrice { get; set; }

    public double? TotalAmount { get; set; }

    public bool? IsPay { get; set; }

    public string IsPayClass { get; set; } = null!;

    public bool? IsActive { get; set; }

    public string? ReasonCancel { get; set; }

    public double? DiscountPercent { get; set; }

    public double? OtherDiscountAmount { get; set; }

    public string? ReasonDiscount { get; set; }

    public double? TotalTime { get; set; }

    public string PromoLink { get; set; } = null!;
}
