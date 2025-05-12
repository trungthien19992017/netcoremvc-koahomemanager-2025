using System;
using System.Collections.Generic;

namespace KOAHome.EntityFramework;

public partial class Abpuser
{
    public int Id { get; set; }

    public int? Accessfailedcount { get; set; }

    public string? Authenticationsource { get; set; }

    public string? Concurrencystamp { get; set; }

    public DateTime? Creationtime { get; set; }

    public long? Creatoruserid { get; set; }

    public long? Deleteruserid { get; set; }

    public DateTime? Deletiontime { get; set; }

    public string? Emailaddress { get; set; }

    public string? Emailconfirmationcode { get; set; }

    public bool? Isactive { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Isemailconfirmed { get; set; }

    public bool? Islockoutenabled { get; set; }

    public bool? Isphonenumberconfirmed { get; set; }

    public bool? Istwofactorenabled { get; set; }

    public DateTime? Lastmodificationtime { get; set; }

    public long? Lastmodifieruserid { get; set; }

    public DateTime? Lockoutenddateutc { get; set; }

    public string? Name { get; set; }

    public string? Normalizedemailaddress { get; set; }

    public string? Normalizedusername { get; set; }

    public string? Password { get; set; }

    public string? Passwordresetcode { get; set; }

    public string? Phonenumber { get; set; }

    public Guid? Profilepictureid { get; set; }

    public string? Securitystamp { get; set; }

    public bool? Shouldchangepasswordonnextlogin { get; set; }

    public string? Surname { get; set; }

    public int? Tenantid { get; set; }

    public string? Username { get; set; }

    public string? Signintoken { get; set; }

    public DateTime? Signintokenexpiretimeutc { get; set; }

    public string? Googleauthenticatorkey { get; set; }

    public string? Position { get; set; }

    public string? Apikey { get; set; }

    public int? Unitid { get; set; }

    public Guid? Useridkcl { get; set; }

    public int? Issynckcl { get; set; }

    public string? Useruid { get; set; }

    public int? Phongbanid { get; set; }

    public string? Useridcchc { get; set; }

    public string? UseridQlvb { get; set; }

    public int? Orderid { get; set; }

    public string? Signatureimg { get; set; }
}
