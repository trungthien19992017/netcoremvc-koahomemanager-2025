using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Abpuserrole
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public int? Roleid { get; set; }

    public int? Tenantid { get; set; }

    public long? Userid { get; set; }

    public int? Rolelevel { get; set; }
}
