using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class HsMediaPlaylist
{
    public int Id { get; set; }

    public int UserManage { get; set; }

    public string? MediaTitle { get; set; }

    public string MediaType { get; set; } = null!;

    public string MediaPath { get; set; } = null!;

    public string? ThumbnailPath { get; set; }

    public int? ThumbnailAttachmentId { get; set; }

    public int? AttachmentId { get; set; }

    public bool? IsChoosen { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }
}
