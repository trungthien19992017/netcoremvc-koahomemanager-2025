using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDisplay
{
    public int Id { get; set; }

    public DateTime? CreationTime { get; set; }

    public long? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public long? LastModifierUserId { get; set; }

    public bool IsDeleted { get; set; }

    public long? DeleterUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public long ReportId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Format { get; set; }

    public string? Type { get; set; }

    public string? Width { get; set; }

    public int ColNum { get; set; }

    public int GroupLevel { get; set; }

    public bool IsDisplay { get; set; }

    public int Order { get; set; }

    public string? TextAlign { get; set; }

    public string? TextIsSum { get; set; }

    public bool IsSum { get; set; }

    public bool? IsCount { get; set; }

    public bool? IsMax { get; set; }

    public bool? IsMin { get; set; }

    public bool? IsFreePane { get; set; }

    public bool IsParent { get; set; }

    public string? ParentCode { get; set; }

    public string? GroupSort { get; set; }

    public bool IsReadOnly { get; set; }

    public bool IsExport { get; set; }

    public int? ServiceId { get; set; }

    public string? ValidationRule { get; set; }

    public string? SortByColumn { get; set; }

    public bool? Visible { get; set; }

    public string? EditCellTemplate { get; set; }

    public string? EditColumns { get; set; }

    public string? ColumnSetData { get; set; }

    public bool? ShowInGroupFooter { get; set; }

    public string? ConfigPopup { get; set; }

    public string? ConfigHeader { get; set; }

    public bool? IsExpand { get; set; }

    public string? Area { get; set; }

    public string? PivotField { get; set; }

    public string? SummaryDisplayMode { get; set; }

    public bool? IsAvg { get; set; }

    public string? PivotOrders { get; set; }

    public string? ReportCode { get; set; }

    public bool? IsSort { get; set; }

    public string? FreePanePosition { get; set; }

    public string? CssHeader { get; set; }

    public string? FormulaSyntax { get; set; }

    public bool AllowMergeCells { get; set; }

    public string? CustomSummary { get; set; }
}
