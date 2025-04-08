using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetMenu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public string? Link { get; set; }

    public string? MobileLink { get; set; }

    public int? Parent { get; set; }

    public int Index { get; set; }

    public string? RequiredPermissionName { get; set; }

    public int MenuId { get; set; }

    public string? SqlString { get; set; }

    public bool? IsCount { get; set; }

    public int CountNum { get; set; }

    public int OrganizationId { get; set; }

    public bool? IsRawSql { get; set; }

    public string? SqlCountStore { get; set; }

    public int CountOutOfDate { get; set; }

    public string? Cssformat { get; set; }

    public string? CssiconFormat { get; set; }

    public int? CountType { get; set; }

    public int ParentOrgId { get; set; }

    public long? UserId { get; set; }

    public string? Code { get; set; }

    public string? Iframe { get; set; }

    public string? ImageUrl { get; set; }

    public string? TextColor { get; set; }

    public int? TypeCheck { get; set; }

    public int? SiteId { get; set; }

    public string? SiteCode { get; set; }
}
