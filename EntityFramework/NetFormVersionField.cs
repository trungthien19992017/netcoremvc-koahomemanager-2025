using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFormVersionfield
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Hinformversionid { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public long? Parentid { get; set; }

    public string? Parentcode { get; set; }

    public long? Fieldtypeid { get; set; }

    public string? Options { get; set; }

    public string? Validates { get; set; }

    public string? Datasources { get; set; }

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public int? Tabindex { get; set; }

    public string? Hinformcode { get; set; }

    public long? Type { get; set; }
}
