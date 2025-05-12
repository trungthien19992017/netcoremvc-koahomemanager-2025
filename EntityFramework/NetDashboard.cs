using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDashboard
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public string? Dashboardcode { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Options { get; set; }

    public string? Storedefault { get; set; }

    public bool? Showcalendarfilter { get; set; }

    public string? Textcalendarcolor { get; set; }

    public bool? Autoreload { get; set; }

    public int? Datasourceid { get; set; }

    public string? Codereceiverealtime { get; set; }
}
