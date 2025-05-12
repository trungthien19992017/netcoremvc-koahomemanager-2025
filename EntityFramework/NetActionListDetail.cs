using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class NetActionlistdetail
{
    public int Id { get; set; }

    public string? Roleid { get; set; }

    public int? Actionlistid { get; set; }

    public string? Actionlistcode { get; set; }

    public int? Actionid { get; set; }

    public string? Value { get; set; }

    public string? Type { get; set; }

    public int? Index { get; set; }

    public string? Icon { get; set; }

    public string? Displayname { get; set; }

    public int? Datasourceid { get; set; }

    public bool? Istop { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public bool? Ischecksamepopup { get; set; }

    public string? Checksamepopuptext { get; set; }

    public string? Checksamepopupbutton { get; set; }

    public string? Urlimportfile { get; set; }

    public string? Filetypeaccept { get; set; }

    public string? Confirmbuttontext { get; set; }

    public string? Confirmtitle { get; set; }

    public string? Confirmtext { get; set; }

    public bool? Ispopupconfirm { get; set; }

    public bool? Ischoosedata { get; set; }

    public bool? Isgroup { get; set; }

    public string? Idgroup { get; set; }

    public string? Errorcol { get; set; }

    public string? Filetemplate { get; set; }

    public string? Typenodediagram { get; set; }

    public int? Siteid { get; set; }

    public string? Sitecode { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? Creationtime { get; set; }

    public int? Creatoruserid { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public int? Lastmodifieruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public int? Deleteuserid { get; set; }

    public int? Orderid { get; set; }

    public bool? Issendrealtime { get; set; }

    public string? Codesendrealtime { get; set; }

    public int? Servicefilename { get; set; }

    public int? Version { get; set; }

    public string? Cssbutton { get; set; }

    public bool? Iszoompopup { get; set; }

    public bool? IsnetActionhowerror { get; set; }
}
