using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetUnit
{
    public int Id { get; set; }

    public string Shortname { get; set; }

    public string Unitcode { get; set; }

    public string Unitname { get; set; }

    public int? Unitgroup { get; set; }

    public long? Parentid { get; set; }

    public int? Orderid { get; set; }

    public int? Leaduserid { get; set; }

    public string Unittype { get; set; }

    public string Description { get; set; }

    public bool? Isparent { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public long? Rootid { get; set; }
}
