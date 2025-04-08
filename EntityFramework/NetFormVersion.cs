using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetFormVersion
{
    public int Id { get; set; }

    public DateTime CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long? HinFormId { get; set; }

    public long? HinFormBookValueId { get; set; }

    public int? Version { get; set; }

    public string? Options { get; set; }

    public string? TableName { get; set; }

    public int? DatasourceId { get; set; }

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public int Type { get; set; }

    public string? ObjectCode { get; set; }

    public string? StoreGetData { get; set; }

    public string? StoreSetData { get; set; }

    public bool? IsBack { get; set; }

    public string? StoreDefaultData { get; set; }

    public bool? IsView { get; set; }

    public string? StoreLabelAction { get; set; }

    public string? StoreSetReadonly { get; set; }

    public string? StoreCheckUrl { get; set; }

    public string? HinFormCode { get; set; }

    public int? PositionButton { get; set; }

    public string? Apicontent { get; set; }

    public bool? ExportMergeField { get; set; }

    public int? SaveEditorType { get; set; }

    public string? ConditionOfAction { get; set; }
}
