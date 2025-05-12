using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetTabpanelDetail
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Hintabpanelid { get; set; }

    public string? Title { get; set; }

    public string? Template { get; set; }

    public string? Options { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isloop { get; set; }

    public string? Storeloop { get; set; }

    public string? Hintabpanelcode { get; set; }

    public string? Tabicon { get; set; }
}
