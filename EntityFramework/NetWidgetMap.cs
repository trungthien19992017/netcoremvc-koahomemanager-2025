using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetMap
{
    public int Id { get; set; }

    public int WidgetItemId { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public int PageId { get; set; }

    public int PositionX { get; set; }

    public int PositionY { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? IndexNumber { get; set; }
}
