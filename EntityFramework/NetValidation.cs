using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetValidation
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Type { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? Pattern { get; set; }

    public string? Store { get; set; }

    public string? Message { get; set; }

    public int? Isactive { get; set; }

    public int? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public int? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public int? Datasourceid { get; set; }
}
