using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFilter
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long ReportId { get; set; }

    public long DynamicFieldId { get; set; }

    public string? Value { get; set; }

    public string? Name { get; set; }

    public int ComboLevel { get; set; }

    public int? ParentComboId { get; set; }

    public string? Code { get; set; }

    public bool DataType { get; set; }

    public long? LookupId { get; set; }

    public long? SeviceId { get; set; }

    public bool Disable { get; set; }

    public bool Required { get; set; }

    public string? Width { get; set; }

    public string? Format { get; set; }

    public int Version { get; set; }

    public int OrderId { get; set; }

    public bool IsActive { get; set; }

    public bool IsValue { get; set; }

    public int? GroupId { get; set; }

    public int? ColSpan { get; set; }

    public int? ColCount { get; set; }

    public bool? IsGrouped { get; set; }

    public string? GroupField { get; set; }

    public bool? IsLoadMultipleWay { get; set; }

    public string? Columns { get; set; }

    public string? ZoomLevel { get; set; }

    public string? DateDisplayFormat { get; set; }

    public string? MultiControlId { get; set; }

    public string? ReportCode { get; set; }

    public int? Type { get; set; }

    public bool? IsFilterToolbar { get; set; }
}
