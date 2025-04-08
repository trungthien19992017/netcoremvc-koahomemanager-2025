using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDataSourceDetail
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long ConfigId { get; set; }

    public int Priority { get; set; }

    public bool IsDefault { get; set; }

    public string? Host { get; set; }

    public string? Dbname { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }

    public int? Timeout { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }
}
