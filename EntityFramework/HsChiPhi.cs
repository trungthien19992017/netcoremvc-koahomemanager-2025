using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsChiphi
{
    public int Id { get; set; }

    public DateTime? Expensedatetime { get; set; }

    public string? Content { get; set; }

    public int? Quantity { get; set; }

    public double? Totalamount { get; set; }

    public bool? Ischeck { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }
}
