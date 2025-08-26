using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetStepperDetail
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long? StepperId { get; set; }

    public long? FormId { get; set; }

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public string? LabelActionCode { get; set; }

    public string? HinWorkflowCode { get; set; }

    public string? SiteCode { get; set; }

    public int? SiteId { get; set; }
}
