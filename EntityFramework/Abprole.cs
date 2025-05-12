using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Abprole
{
    public int Id { get; set; }

    public string? Concurrencystamp { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Displayname { get; set; }

    public bool? Isdefault { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Isstatic { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public string? Name { get; set; }

    public string? Normalizedname { get; set; }

    public int? Tenantid { get; set; }

    public string? Pageredirect { get; set; }

    public string? Code { get; set; }

    public string? Loai { get; set; }

    public string? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public int? Orderid { get; set; }

    public int? Defaultmenuid { get; set; }

    public string? Description { get; set; }

    public int? Unitid { get; set; }

    public int? Parentid { get; set; }

    public int? Roletype { get; set; }

    public string? Labelid { get; set; }
}
