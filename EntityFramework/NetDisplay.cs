using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetDisplay
{
    public int Id { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public bool? Isdeleted { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public long? Reportid { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Format { get; set; }

    public string? Type { get; set; }

    public string? Width { get; set; }

    public int? Colnum { get; set; }

    public int? Grouplevel { get; set; }

    public bool? Isdisplay { get; set; }

    public int? Order { get; set; }

    public string? Textalign { get; set; }

    public string? Textissum { get; set; }

    public bool? Issum { get; set; }

    public bool? Iscount { get; set; }

    public bool? Ismax { get; set; }

    public bool? Ismin { get; set; }

    public bool? Isfreepane { get; set; }

    public bool? Isparent { get; set; }

    public string? Parentcode { get; set; }

    public string? Groupsort { get; set; }

    public bool? Isreadonly { get; set; }

    public bool? Isexport { get; set; }

    public int? Serviceid { get; set; }

    public string? Validationrule { get; set; }

    public string? Sortbycolumn { get; set; }

    public bool? Visible { get; set; }

    public string? Editcelltemplate { get; set; }

    public string? Editcolumns { get; set; }

    public string? Columnsetdata { get; set; }

    public bool? Showingroupfooter { get; set; }

    public string? Configpopup { get; set; }

    public string? Configheader { get; set; }

    public bool? Isexpand { get; set; }

    public string? Area { get; set; }

    public string? Pivotfield { get; set; }

    public string? Summarydisplaymode { get; set; }

    public bool? Isavg { get; set; }

    public string? Pivotorders { get; set; }

    public string? Reportcode { get; set; }

    public bool? Issort { get; set; }

    public string? Freepaneposition { get; set; }

    public string? Cssheader { get; set; }

    public string? Formulasyntax { get; set; }

    public bool? Allowmergecells { get; set; }

    public string? Customsummary { get; set; }
}
