using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetmap
{
    public int Id { get; set; }

    public int? Widgetitemid { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public int? Pageid { get; set; }

    public int? Positionx { get; set; }

    public int? Positiony { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Indexnumber { get; set; }
}
