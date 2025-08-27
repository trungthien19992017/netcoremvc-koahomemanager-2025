using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetStepper
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

    public long? OrderId { get; set; }

    public bool? IsActive { get; set; }

    public int? DatasourceId { get; set; }

    public string? StoreGetData { get; set; }

    public string? StoreSetData { get; set; }

    public string? StoreDefaultData { get; set; }

    public bool? IsDynamicData { get; set; }

    public string? StoreLoadDynamicData { get; set; }

    public bool? IsSaveEachForm { get; set; }

    public bool? Vertical { get; set; }

    public bool? IsViewOnly { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public bool? IsActiveEventHeader { get; set; }
}
