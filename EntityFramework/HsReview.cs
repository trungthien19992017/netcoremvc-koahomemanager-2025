using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsReview
{
    public int Reviewid { get; set; }

    public int? Customerid { get; set; }

    public int? Roomid { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? Reviewdate { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public virtual HsCustomer? Customer { get; set; }

    public virtual HsRoom? Room { get; set; }
}
