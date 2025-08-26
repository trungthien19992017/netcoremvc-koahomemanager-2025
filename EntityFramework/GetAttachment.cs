using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Getattachment
{
    public int? Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Tenantid { get; set; }

    public string? Filename { get; set; }

    public string? Diskfilename { get; set; }

    public int? Filesize { get; set; }

    public string? Contenttype { get; set; }

    public long? Authorid { get; set; }

    public string? Diskdirectory { get; set; }

    public long? Objectid { get; set; }

    public long? Objecttypeid { get; set; }

    public bool? Iscurrent { get; set; }

    public int? Version { get; set; }

    public string? Convertfilename { get; set; }

    public string? Convertdiskdirectory { get; set; }

    public int? Order { get; set; }
}
