using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HinCategorydetail
{
    public int? Id { get; set; }

    public int? Categoryid { get; set; }

    public string? Categorycode { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Orderid { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }
}
