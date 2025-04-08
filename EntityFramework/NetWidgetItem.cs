using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetWidgetItem
{
    public int Id { get; set; }

    public int WidgetLayoutId { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int GroupWidgetId { get; set; }

    public string? ImgReview { get; set; }

    public int? DataSourceId { get; set; }

    public string? TemplateIds { get; set; }
}
