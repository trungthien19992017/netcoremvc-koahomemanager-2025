using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetTabPanelDetail
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long? HinTabPanelId { get; set; }

    public string? Title { get; set; }

    public string? Template { get; set; }

    public string? Options { get; set; }

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsLoop { get; set; }

    public string? StoreLoop { get; set; }

    public string? HinTabPanelCode { get; set; }
}
