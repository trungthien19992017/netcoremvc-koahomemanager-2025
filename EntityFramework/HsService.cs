using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsService
{
    public int Serviceid { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Ishourservice { get; set; }

    public string? Formula { get; set; }

    public double? Price { get; set; }

    public bool? Ispricebyroom { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public string? Fromhour { get; set; }

    public string? Tohour { get; set; }

    public int? Orderid { get; set; }

    public string? Serviceimage { get; set; }

    public int? Intimerange { get; set; }

    public bool? Isaddon { get; set; }

    public string? Applydate { get; set; }

    public virtual ICollection<HsBookingservice> HsBookingservices { get; set; } = new List<HsBookingservice>();
}
