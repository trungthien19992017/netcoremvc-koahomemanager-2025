using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidget
{
    public int Id { get; set; }

    public string? Widgetcode { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Displaytypecode { get; set; }
}
