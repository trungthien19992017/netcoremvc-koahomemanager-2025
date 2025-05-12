using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFormfieldtype
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

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isrowtemplate { get; set; }
}
