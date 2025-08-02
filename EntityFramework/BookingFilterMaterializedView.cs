using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class BookingFilterMaterializedView
{
    public int Id { get; set; }

    public string? PhoneNumber { get; set; }

    public string LastName { get; set; } = null!;

    public DateTime CheckInDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public int RoomId { get; set; }
}
