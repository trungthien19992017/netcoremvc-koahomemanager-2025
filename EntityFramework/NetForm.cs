using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetForm
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

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? Latestversion { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public string? Cssoptionheader { get; set; }

    public string? Codereceiverealtime { get; set; }

    public bool? Issendrealtime { get; set; }

    public bool? Isreceiverealtime { get; set; }

    public string? Codesendrealtime { get; set; }
}
