using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetReport
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? Sqltype { get; set; }

    public string? Sqlcontent { get; set; }

    public long? Grouplevel { get; set; }

    public string? Excel { get; set; }

    public long? Datasourceid { get; set; }

    public string? Layoutpfilter { get; set; }

    public int? Displaytype { get; set; }

    public bool? Isdynamiccolumn { get; set; }

    public long? Formid { get; set; }

    public int? Typegetcolumn { get; set; }

    public bool? Isexportword { get; set; }

    public string? Word { get; set; }

    public string? Sqleditcontent { get; set; }

    public string? Sqldefaultcontent { get; set; }

    public string? Sqlstoredlabelaction { get; set; }

    public bool? Disablesearch { get; set; }

    public int? Colspan { get; set; }

    public int? Colcount { get; set; }

    public string? Sqledittemplatecontent { get; set; }

    public string? Sqlexportdata { get; set; }

    public string? Sqlexportfield { get; set; }

    public string? Allowedpagesizes { get; set; }

    public bool? Disablehandlecollumn { get; set; }

    public string? Storedrdisplay { get; set; }

    public bool? Isautocollapse { get; set; }

    public bool? Sqltypem { get; set; }

    public string? Sqlcontentm { get; set; }

    public bool? Isediteditor { get; set; }

    public bool? Iscreateeditor { get; set; }

    public bool? Isbtnhandle { get; set; }

    public bool? Isexportexcel { get; set; }

    public string? Selectiontype { get; set; }

    public string? Storecheckurl { get; set; }

    public bool? Isbackviewer { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public bool? Issearchbar { get; set; }

    public bool? Isfreepane { get; set; }

    public string? Defaultparam { get; set; }

    public int? Positionbutton { get; set; }

    public int? Reporttype { get; set; }

    public string? Storedrag { get; set; }

    public string? Editingmode { get; set; }

    public bool? Showheaderfilter { get; set; }

    public string? Functioncode { get; set; }

    public int? Funtionid { get; set; }

    public bool? Showpage { get; set; }

    public bool? Showtoolbar { get; set; }

    public int? Chartviewdisplay { get; set; }

    public string? Reportcoderecieverealtime { get; set; }

    public bool? Showiconfilter { get; set; }

    public bool? Isrecieverealtime { get; set; }

    public bool? Isdeleteeditor { get; set; }

    public int? Servicehiddenfilter { get; set; }

    public string? Templateids { get; set; }

    public bool? Cache { get; set; }

    public string? Allowedapi { get; set; }

    public bool? Pagination { get; set; }

    public bool? Enablemasterdetail { get; set; }

    public string? Masterdetailreportcode { get; set; }
}
