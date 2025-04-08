using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetAttachmentSyntax
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

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? SyntaxPath { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsChangeSyntaxName { get; set; }

    public string? SyntaxName { get; set; }

    public int? OrderId { get; set; }

    public int? DeleteUserId { get; set; }
}
