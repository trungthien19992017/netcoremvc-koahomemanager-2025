using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDashboardPage
{
    public int Id { get; set; }

    public string? PageCode { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public int? OrderId { get; set; }

    public int DashboardId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }
}
