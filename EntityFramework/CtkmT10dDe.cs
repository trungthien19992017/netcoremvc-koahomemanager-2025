using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class CtkmT10dDe
{
    public int Id { get; set; }

    public int? Customerid { get; set; }

    public int? Time { get; set; }

    public int? Kmid { get; set; }

    public string? Kmcode { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }
}
