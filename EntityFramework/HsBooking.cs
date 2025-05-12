using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsBooking
{
    public int Bookingid { get; set; }

    public int? Customerid { get; set; }

    public int? Roomid { get; set; }

    public DateTime? Checkindate { get; set; }

    public DateTime? Checkoutdate { get; set; }

    public DateTime? Bookingdate { get; set; }

    public double? Unitprice { get; set; }

    public double? Totalamount { get; set; }

    public double? Deposit { get; set; }

    public double? Totaltime { get; set; }

    public string? Otherphonenumber { get; set; }

    public double? Discountpercent { get; set; }

    public double? Otherdiscountamount { get; set; }

    public string? Reasondiscount { get; set; }

    public bool? Ispay { get; set; }

    public string? Reasoncancel { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual HsCustomer? Customer { get; set; }

    public virtual ICollection<HsBookingservice> HsBookingservices { get; set; } = new List<HsBookingservice>();

    public virtual ICollection<HsPayment> HsPayments { get; set; } = new List<HsPayment>();

    public virtual HsRoom? Room { get; set; }
}
