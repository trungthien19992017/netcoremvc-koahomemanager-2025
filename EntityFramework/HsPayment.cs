using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsPayment
{
    public int Paymentid { get; set; }

    public int? Bookingid { get; set; }

    public double? Amount { get; set; }

    public DateTime? Paymentdate { get; set; }

    public string? Paymentmethod { get; set; }

    public string? Paymentinformation { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual HsBooking? Booking { get; set; }
}
