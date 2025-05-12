using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFormVersion
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Hinformid { get; set; }

    public long? Hinformbookvalueid { get; set; }

    public int? Version { get; set; }

    public string? Options { get; set; }

    public string? Tablename { get; set; }

    public int? Datasourceid { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public int? Type { get; set; }

    public string? Objectcode { get; set; }

    public string? Storegetdata { get; set; }

    public string? Storesetdata { get; set; }

    public bool? Isback { get; set; }

    public string? Storedefaultdata { get; set; }

    public bool? Isview { get; set; }

    public string? Storelabelaction { get; set; }

    public string? Storesetreadonly { get; set; }

    public string? Storecheckurl { get; set; }

    public string? Hinformcode { get; set; }

    public int? Positionbutton { get; set; }

    public string? Apicontent { get; set; }

    public bool? Exportmergefield { get; set; }

    public int? Saveeditortype { get; set; }

    public string? Conditionofaction { get; set; }
}
