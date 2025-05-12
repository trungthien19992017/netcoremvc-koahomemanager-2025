using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsBookingservice
{
    public int Bookingserviceid { get; set; }

    public int? Bookingid { get; set; }

    public int? Serviceid { get; set; }

    public int? Quantity { get; set; }

    public double? Totalprice { get; set; }

    public DateTime? Additionfromdate { get; set; }

    public DateTime? Additiontodate { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual HsBooking? Booking { get; set; }

    public virtual HsService? Service { get; set; }
}
