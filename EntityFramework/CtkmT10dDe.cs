using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class CtkmT10dDe
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? Time { get; set; }

    public int? Kmid { get; set; }

    public string? Kmcode { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
