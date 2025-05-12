using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFilter
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Reportid { get; set; }

    public long? Dynamicfieldid { get; set; }

    public string? Value { get; set; }

    public string? Name { get; set; }

    public int? Combolevel { get; set; }

    public int? Parentcomboid { get; set; }

    public string? Code { get; set; }

    public bool? Datatype { get; set; }

    public long? Lookupid { get; set; }

    public long? Seviceid { get; set; }

    public bool? Disable { get; set; }

    public bool? Required { get; set; }

    public string? Width { get; set; }

    public string? Format { get; set; }

    public int? Version { get; set; }

    public int? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isvalue { get; set; }

    public int? Groupid { get; set; }

    public int? Colspan { get; set; }

    public int? Colcount { get; set; }

    public bool? Isgrouped { get; set; }

    public string? Groupfield { get; set; }

    public bool? Isloadmultipleway { get; set; }

    public string? Columns { get; set; }

    public string? Zoomlevel { get; set; }

    public string? Datedisplayformat { get; set; }

    public string? Multicontrolid { get; set; }

    public string? Reportcode { get; set; }

    public int? Type { get; set; }

    public bool? Isfiltertoolbar { get; set; }
}
