using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsChiPhi
{
    public int Id { get; set; }

    public DateTime ExpenseDatetime { get; set; }

    public string Content { get; set; } = null!;

    public int? Quantity { get; set; }

    public double? TotalAmount { get; set; }

    public bool? IsCheck { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
