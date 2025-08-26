using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsServicepricebyroom
{
    public int Id { get; set; }

    public int? Roomid { get; set; }

    public int? Serviceid { get; set; }

    public int? Quantity { get; set; }

    public decimal? Serviceprice { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }
}
