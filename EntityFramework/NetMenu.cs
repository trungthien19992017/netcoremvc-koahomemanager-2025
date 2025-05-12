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

    public string? Mobilelink { get; set; }

    public int? Parent { get; set; }

    public int? Index { get; set; }

    public string? Requiredpermissionname { get; set; }

    public int? Menuid { get; set; }

    public string? Sqlstring { get; set; }

    public bool? Iscount { get; set; }

    public int? Countnum { get; set; }

    public int? Organizationid { get; set; }

    public bool? Israwsql { get; set; }

    public string? Sqlcountstore { get; set; }

    public int? Countoutofdate { get; set; }

    public string? Cssformat { get; set; }

    public string? Cssiconformat { get; set; }

    public int? Counttype { get; set; }

    public int? Parentorgid { get; set; }

    public long? Userid { get; set; }

    public string? Code { get; set; }

    public string? Iframe { get; set; }

    public string? Imageurl { get; set; }

    public string? Textcolor { get; set; }

    public int? Typecheck { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public bool? Isdeleted { get; set; }
}
