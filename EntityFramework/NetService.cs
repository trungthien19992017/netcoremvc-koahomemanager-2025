using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetService
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public long DataSourceId { get; set; }

    public bool SqlType { get; set; }

    public string? SqlContent { get; set; }

    public string? ColValue { get; set; }

    public string? ColDisplay { get; set; }

    public string? ColParent { get; set; }

    public bool? Cache { get; set; }

    public string? StoredDefaultParam { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }
}
