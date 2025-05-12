using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetService
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

    public long? Datasourceid { get; set; }

    public bool? Sqltype { get; set; }

    public string? Sqlcontent { get; set; }

    public string? Colvalue { get; set; }

    public string? Coldisplay { get; set; }

    public string? Colparent { get; set; }

    public bool? Cache { get; set; }

    public string? Storeddefaultparam { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }
}
