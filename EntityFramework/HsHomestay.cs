using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsHomestay
{
    public int Homestayid { get; set; }

    public int? Ownerid { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual ICollection<HsRoom> HsRooms { get; set; } = new List<HsRoom>();

    public virtual HsOwner? Owner { get; set; }
}
