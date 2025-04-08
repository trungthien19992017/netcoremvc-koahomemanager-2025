using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDashboard
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public string? DashboardCode { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string? Options { get; set; }

    public string? StoreDefault { get; set; }

    public bool ShowCalendarFilter { get; set; }

    public string? TextCalendarColor { get; set; }

    public bool AutoReload { get; set; }

    public int? DataSourceId { get; set; }

    public string? CodeReceiveRealTime { get; set; }
}
