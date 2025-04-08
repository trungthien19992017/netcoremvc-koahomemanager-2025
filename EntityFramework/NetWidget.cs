using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidget
{
    public int Id { get; set; }

    public string? WidgetCode { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string? DisplayTypeCode { get; set; }
}
