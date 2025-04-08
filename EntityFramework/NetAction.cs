using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetAction
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDelete { get; set; }

    public string? Icon { get; set; }

    public bool? IsPopupConfirm { get; set; }

    public string? ConfirmTitle { get; set; }

    public string? ConfirmText { get; set; }

    public string? ConfirmButtonText { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }
}
