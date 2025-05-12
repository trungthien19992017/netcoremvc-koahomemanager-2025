using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetMainmenu
{
    public int Id { get; set; }

    public int? Tenantid { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Icon { get; set; }

    public string? Description { get; set; }

    public int? Parent { get; set; }

    public string? Link { get; set; }

    public DateTime? Creationtime { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Index { get; set; }

    public string? Requiredpermissionname { get; set; }

    public long? Creatoruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public int? Devicetype { get; set; }

    public bool? Ismobile { get; set; }

    public string? Code { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public bool? Isminiitem { get; set; }

    public string? Imageurl { get; set; }
}
