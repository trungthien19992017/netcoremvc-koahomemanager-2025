using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class CtkmT10d
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? DiemTu { get; set; }

    public int? DiemDen { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public bool? HaveVoucher { get; set; }
}
