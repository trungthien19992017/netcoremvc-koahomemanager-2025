using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetForm
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

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? LatestVersion { get; set; }

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public string? CssOptionHeader { get; set; }

    public string? CodeReceiveRealTime { get; set; }

    public bool? IsSendRealTime { get; set; }

    public bool? IsReceiveRealTime { get; set; }

    public string? CodeSendRealTime { get; set; }
}
