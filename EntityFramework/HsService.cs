using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsService
{
    public int ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsHourService { get; set; }

    public string? Formula { get; set; }

    public double? Price { get; set; }

    public bool? IsPriceByRoom { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public string? FromHour { get; set; }

    public string? ToHour { get; set; }

    public int? OrderId { get; set; }

    public string? ServiceImage { get; set; }

    public virtual ICollection<HsBookingService> HsBookingServices { get; set; } = new List<HsBookingService>();
}
