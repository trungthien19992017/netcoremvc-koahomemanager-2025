using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetReport
{
    public int Id { get; set; }

    public DateTime? CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool? IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? SqlType { get; set; }

    public string? SqlContent { get; set; }

    public long? GroupLevel { get; set; }

    public string? Excel { get; set; }

    public long? DataSourceId { get; set; }

    public string? LayoutpFilter { get; set; }

    public int? DisplayType { get; set; }

    public bool? IsDynamicColumn { get; set; }

    public long? FormId { get; set; }

    public int? TypeGetColumn { get; set; }

    public bool? IsExportWord { get; set; }

    public string? Word { get; set; }

    public string? SqlEditContent { get; set; }

    public string? SqlDefaultContent { get; set; }

    public string? SqlStoredLabelAction { get; set; }

    public bool? DisableSearch { get; set; }

    public int? ColSpan { get; set; }

    public int? ColCount { get; set; }

    public string? SqlEditTemplateContent { get; set; }

    public string? SqlExportData { get; set; }

    public string? SqlExportField { get; set; }

    public string? AllowedPageSizes { get; set; }

    public bool? DisableHandleCollumn { get; set; }

    public string? StoreDrdisplay { get; set; }

    public bool? IsAutoCollapse { get; set; }

    public bool? SqlTypeM { get; set; }

    public string? SqlContentM { get; set; }

    public bool? IsEditEditor { get; set; }

    public bool? IsCreateEditor { get; set; }

    public bool? IsBtnHandle { get; set; }

    public bool? IsExportExcel { get; set; }

    public string? SelectionType { get; set; }

    public string? StoreCheckUrl { get; set; }

    public bool? IsBackViewer { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public bool? IsSearchbar { get; set; }

    public bool? IsFreepane { get; set; }

    public string? DefaultParam { get; set; }

    public int? PositionButton { get; set; }

    public int? ReportType { get; set; }

    public string? StoreDrag { get; set; }

    public string? EditingMode { get; set; }

    public bool? ShowHeaderFilter { get; set; }

    public string? FunctionCode { get; set; }

    public int? FuntionId { get; set; }

    public bool? ShowPage { get; set; }

    public bool? ShowToolbar { get; set; }

    public int? ChartViewDisplay { get; set; }

    public string? ReportCodeRecieveRealTime { get; set; }

    public bool? ShowIconFilter { get; set; }

    public bool? IsRecieveRealTime { get; set; }

    public bool? IsDeleteEditor { get; set; }

    public int? ServiceHiddenFilter { get; set; }

    public string? TemplateIds { get; set; }

    public bool? Cache { get; set; }

    public string? AllowedApi { get; set; }

    public bool? Pagination { get; set; }

    public bool EnableMasterDetail { get; set; }

    public string? MasterDetailReportCode { get; set; }
}
