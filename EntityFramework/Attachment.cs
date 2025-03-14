using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Attachment
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? TenantId { get; set; }

    public string? FileName { get; set; }

    public string? DiskFileName { get; set; }

    public int? FileSize { get; set; }

    public string? ContentType { get; set; }

    public long? AuthorId { get; set; }

    public string? DiskDirectory { get; set; }

    public long? ObjectId { get; set; }

    public long? ObjectTypeId { get; set; }

    public bool IsCurrent { get; set; }

    public int? Version { get; set; }

    public string? ConvertFileName { get; set; }

    public string? ConvertDiskDirectory { get; set; }

    public int? Order { get; set; }
}
