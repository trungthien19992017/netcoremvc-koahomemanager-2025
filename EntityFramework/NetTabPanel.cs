using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetTabPanel
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

    public string? StoreGetData { get; set; }

    public int? DatasourceId { get; set; }

    public string? FileTemplate { get; set; }

    public string? StoreExportFile { get; set; }

    public int? SelectedIndex { get; set; }

    public bool? IsEffectIcon { get; set; }

    public string? StoreCheckTabDetail { get; set; }

    public string? BeforeEffectIcon { get; set; }

    public string? BeforeEffectIconColor { get; set; }

    public string? AfterEffectIcon { get; set; }

    public string? AfterEffectIconColor { get; set; }

    public bool? IsPermission { get; set; }

    public bool? IsPermissionByRecord { get; set; }

    public string? StorePermissionByRecord { get; set; }

    public string? StoreCheckUrl { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public string? StoreGetFieldExportForm { get; set; }

    public string? StoreGetFieldExportDatagrid { get; set; }

    public bool? IsExportExcel { get; set; }

    public bool? IsExportWordTemplate { get; set; }

    public string? StoreCountNotify { get; set; }

    public string? StoreTabPermission { get; set; }

    public int? ServiceGetFileName { get; set; }
}
