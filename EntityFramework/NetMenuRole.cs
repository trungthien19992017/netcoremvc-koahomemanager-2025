using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetMenuRole
{
    public int Id { get; set; }

    public int? TenantId { get; set; }

    public int RoleId { get; set; }

    public int LabelId { get; set; }

    public int MenuId { get; set; }

    public bool IsActive { get; set; }

    public int? RoleMapperGroupId { get; set; }

    public int? Order { get; set; }
}
