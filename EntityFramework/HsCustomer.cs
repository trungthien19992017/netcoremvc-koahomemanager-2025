using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsCustomer
{
    public int Customerid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public DateTime? Dateofbirth { get; set; }

    public string? Gender { get; set; }

    public string? Cccd { get; set; }

    public string? Description { get; set; }

    public string? Mxh { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual ICollection<HsBooking> HsBookings { get; set; } = new List<HsBooking>();

    public virtual ICollection<HsReview> HsReviews { get; set; } = new List<HsReview>();
}
