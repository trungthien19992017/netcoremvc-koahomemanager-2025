using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsServiceHistory
{
    public int Id { get; set; }

    public string? Tablename { get; set; }

    public int? Objectid { get; set; }

    public string? Information { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public bool? Isread { get; set; }
}
