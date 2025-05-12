using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetitem
{
    public int Id { get; set; }

    public int? Widgetlayoutid { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Groupwidgetid { get; set; }

    public string? Imgreview { get; set; }

    public int? Datasourceid { get; set; }

    public string? Templateids { get; set; }
}
