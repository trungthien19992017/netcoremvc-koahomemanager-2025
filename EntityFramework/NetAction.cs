using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetAction
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Descriptions { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdelete { get; set; }

    public string? Icon { get; set; }

    public bool? Ispopupconfirm { get; set; }

    public string? Confirmtitle { get; set; }

    public string? Confirmtext { get; set; }

    public string? Confirmbuttontext { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }
}
