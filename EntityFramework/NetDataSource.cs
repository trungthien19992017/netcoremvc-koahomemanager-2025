using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDataSource
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

    public int Type { get; set; }

    public string? SqlType { get; set; }

    public string? SiteCode { get; set; }

    public int? SiteId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }
}
