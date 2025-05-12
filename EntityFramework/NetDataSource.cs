using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDatasource
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Type { get; set; }

    public string? Sqltype { get; set; }

    public string? Sitecode { get; set; }

    public int? Siteid { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
