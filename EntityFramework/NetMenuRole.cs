using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetMenurole
{
    public int Id { get; set; }

    public int? Tenantid { get; set; }

    public int? Roleid { get; set; }

    public int? Labelid { get; set; }

    public int? Menuid { get; set; }

    public bool? Isactive { get; set; }

    public int? Rolemappergroupid { get; set; }

    public int? Order { get; set; }
}
