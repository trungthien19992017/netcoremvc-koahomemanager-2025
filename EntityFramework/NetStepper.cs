using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetStepper
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public int? Datasourceid { get; set; }

    public string Storegetdata { get; set; }

    public string Storesetdata { get; set; }

    public string Storedefaultdata { get; set; }

    public bool? Isdynamicdata { get; set; }

    public string Storeloaddynamicdata { get; set; }

    public bool? Issaveeachform { get; set; }

    public bool? Vertical { get; set; }

    public bool? Isviewonly { get; set; }

    public int? Siteid { get; set; }

    public string Sitecode { get; set; }

    public bool? Isactiveeventheader { get; set; }
}
