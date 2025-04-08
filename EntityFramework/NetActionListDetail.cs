using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetActionListDetail
{
    public int Id { get; set; }

    public string? RoleId { get; set; }

    public int ActionListId { get; set; }

    public string? ActionListCode { get; set; }

    public int ActionId { get; set; }

    public string? Value { get; set; }

    public string? Type { get; set; }

    public int? Index { get; set; }

    public string? Icon { get; set; }

    public string? DisplayName { get; set; }

    public int? DataSourceId { get; set; }

    public bool? IsTop { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public bool? IsCheckSamePopup { get; set; }

    public string? CheckSamePopupText { get; set; }

    public string? CheckSamePopupButton { get; set; }

    public string? UrlImportFile { get; set; }

    public string? FileTypeAccept { get; set; }

    public string? ConfirmButtonText { get; set; }

    public string? ConfirmTitle { get; set; }

    public string? ConfirmText { get; set; }

    public bool? IsPopupConfirm { get; set; }

    public bool? IsChooseData { get; set; }

    public bool? IsGroup { get; set; }

    public string? Idgroup { get; set; }

    public string? ErrorCol { get; set; }

    public string? FileTemplate { get; set; }

    public string? TypeNodeDiagram { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationTime { get; set; }

    public int? CreatorUserId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int? LastModifierUserId { get; set; }

    public DateTime? DeletionTime { get; set; }

    public int? DeleteUserId { get; set; }

    public int? OrderId { get; set; }

    public bool? IsSendRealTime { get; set; }

    public string? CodeSendRealTime { get; set; }

    public int? ServiceFileName { get; set; }

    public int? Version { get; set; }

    public string? CssButton { get; set; }

    public bool IsZoomPopup { get; set; }

    public bool? IsNetActionhowError { get; set; }
}
