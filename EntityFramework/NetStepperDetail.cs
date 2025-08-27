using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetStepperDetail
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Stepperid { get; set; }

    public long? Formid { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public string Labelactioncode { get; set; }

    public string Hinworkflowcode { get; set; }

    public string Sitecode { get; set; }

    public int? Siteid { get; set; }
}
