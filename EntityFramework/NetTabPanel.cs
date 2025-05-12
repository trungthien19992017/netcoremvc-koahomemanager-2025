using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetTabpanel
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

    public long? Orderid { get; set; }

    public bool? Isactive { get; set; }

    public string? Storegetdata { get; set; }

    public int? Datasourceid { get; set; }

    public string? Filetemplate { get; set; }

    public string? Storeexportfile { get; set; }

    public int? Selectedindex { get; set; }

    public bool? Iseffecticon { get; set; }

    public string? Storechecktabdetail { get; set; }

    public string? Beforeeffecticon { get; set; }

    public string? Beforeeffecticoncolor { get; set; }

    public string? Aftereffecticon { get; set; }

    public string? Aftereffecticoncolor { get; set; }

    public bool? Ispermission { get; set; }

    public bool? Ispermissionbyrecord { get; set; }

    public string? Storepermissionbyrecord { get; set; }

    public string? Storecheckurl { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public string? Storegetfieldexportform { get; set; }

    public string? Storegetfieldexportdatagrid { get; set; }

    public bool? Isexportexcel { get; set; }

    public bool? Isexportwordtemplate { get; set; }

    public string? Storecountnotify { get; set; }

    public string? Storetabpermission { get; set; }

    public int? Servicegetfilename { get; set; }
}
