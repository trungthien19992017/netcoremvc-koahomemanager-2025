using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFormVersionField
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long? HinFormVersionId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public long? ParentId { get; set; }

    public string? ParentCode { get; set; }

    public long? FieldTypeId { get; set; }

    public string? Options { get; set; }

    public string? Validates { get; set; }

    public string? Datasources { get; set; }

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public int? TabIndex { get; set; }

    public string? HinFormCode { get; set; }

    public long? Type { get; set; }
}
