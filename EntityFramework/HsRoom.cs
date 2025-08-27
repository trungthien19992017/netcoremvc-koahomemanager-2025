using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsRoom
{
    public int Roomid { get; set; }

    public int? Homestayid { get; set; }

    public string? Name { get; set; }

    public string? Number { get; set; }

    public int? Floor { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public string? Color { get; set; }

    public string? Badgeclass { get; set; }

    public string? Roomimage { get; set; }

    public virtual HsHomestay? Homestay { get; set; }

    public virtual ICollection<HsBooking> HsBookings { get; set; } = new List<HsBooking>();

    public virtual ICollection<HsReview> HsReviews { get; set; } = new List<HsReview>();
}
