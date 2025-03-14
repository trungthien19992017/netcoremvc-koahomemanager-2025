using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsThongTinDatView
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public string CheckInInfo { get; set; } = null!;
}
