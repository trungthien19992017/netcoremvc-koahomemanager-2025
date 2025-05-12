using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KOAHome.EntityFramework;

public partial class QLKCL_NEWContext : DbContext
{
    public QLKCL_NEWContext()
    {
    }

    public QLKCL_NEWContext(DbContextOptions<QLKCL_NEWContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abprole> Abproles { get; set; }

    public virtual DbSet<Abpuser> Abpusers { get; set; }

    public virtual DbSet<Abpuserrole> Abpuserroles { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Attachmentsyntax> Attachmentsyntaxes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Categorydetail> Categorydetails { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CtkmT10d> CtkmT10ds { get; set; }

    public virtual DbSet<CtkmT10dDe> CtkmT10dDes { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Drdisplay> Drdisplays { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Getattachment> Getattachments { get; set; }

    public virtual DbSet<HinCategory> HinCategories { get; set; }

    public virtual DbSet<HinCategorydetail> HinCategorydetails { get; set; }

    public virtual DbSet<HinCity> HinCities { get; set; }

    public virtual DbSet<HinDistrict> HinDistricts { get; set; }

    public virtual DbSet<HinWard> HinWards { get; set; }

    public virtual DbSet<HsBooking> HsBookings { get; set; }

    public virtual DbSet<HsBookingservice> HsBookingservices { get; set; }

    public virtual DbSet<HsChiphi> HsChiphis { get; set; }

    public virtual DbSet<HsCustomer> HsCustomers { get; set; }

    public virtual DbSet<HsDichvutheolich> HsDichvutheoliches { get; set; }

    public virtual DbSet<HsHomestay> HsHomestays { get; set; }

    public virtual DbSet<HsOwner> HsOwners { get; set; }

    public virtual DbSet<HsPayment> HsPayments { get; set; }

    public virtual DbSet<HsReview> HsReviews { get; set; }

    public virtual DbSet<HsRoom> HsRooms { get; set; }

    public virtual DbSet<HsService> HsServices { get; set; }

    public virtual DbSet<HsServiceHistory> HsServiceHistories { get; set; }

    public virtual DbSet<HsServicepricebyroom> HsServicepricebyrooms { get; set; }

    public virtual DbSet<HsThongtindatView> HsThongtindatViews { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Nationality> Nationalities { get; set; }

    public virtual DbSet<Radioyesno> Radioyesnos { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("dblink")
            .HasPostgresExtension("postgres_fdw");

        modelBuilder.Entity<Abprole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_abproles");

            entity.ToTable("abproles", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Concurrencystamp)
                .HasMaxLength(128)
                .HasColumnName("concurrencystamp");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Defaultmenuid).HasColumnName("defaultmenuid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Displayname)
                .HasMaxLength(64)
                .HasColumnName("displayname");
            entity.Property(e => e.Isdefault).HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isstatic).HasColumnName("isstatic");
            entity.Property(e => e.Labelid)
                .HasMaxLength(50)
                .HasColumnName("labelid");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Loai)
                .HasMaxLength(100)
                .HasColumnName("loai");
            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");
            entity.Property(e => e.Normalizedname)
                .HasMaxLength(32)
                .HasColumnName("normalizedname");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Pageredirect).HasColumnName("pageredirect");
            entity.Property(e => e.Parentid).HasColumnName("parentid");
            entity.Property(e => e.Roletype).HasColumnName("roletype");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid)
                .HasMaxLength(50)
                .HasColumnName("siteid");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Unitid).HasColumnName("unitid");
        });

        modelBuilder.Entity<Abpuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_abpusers");

            entity.ToTable("abpusers", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Accessfailedcount).HasColumnName("accessfailedcount");
            entity.Property(e => e.Apikey).HasColumnName("apikey");
            entity.Property(e => e.Authenticationsource)
                .HasMaxLength(64)
                .HasColumnName("authenticationsource");
            entity.Property(e => e.Concurrencystamp)
                .HasMaxLength(128)
                .HasColumnName("concurrencystamp");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Emailaddress)
                .HasMaxLength(256)
                .HasColumnName("emailaddress");
            entity.Property(e => e.Emailconfirmationcode)
                .HasMaxLength(328)
                .HasColumnName("emailconfirmationcode");
            entity.Property(e => e.Googleauthenticatorkey).HasColumnName("googleauthenticatorkey");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isemailconfirmed).HasColumnName("isemailconfirmed");
            entity.Property(e => e.Islockoutenabled).HasColumnName("islockoutenabled");
            entity.Property(e => e.Isphonenumberconfirmed).HasColumnName("isphonenumberconfirmed");
            entity.Property(e => e.Issynckcl).HasColumnName("issynckcl");
            entity.Property(e => e.Istwofactorenabled).HasColumnName("istwofactorenabled");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Lockoutenddateutc).HasColumnName("lockoutenddateutc");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Normalizedemailaddress)
                .HasMaxLength(256)
                .HasColumnName("normalizedemailaddress");
            entity.Property(e => e.Normalizedusername)
                .HasMaxLength(256)
                .HasColumnName("normalizedusername");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Passwordresetcode)
                .HasMaxLength(328)
                .HasColumnName("passwordresetcode");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(32)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Phongbanid).HasColumnName("phongbanid");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Profilepictureid).HasColumnName("profilepictureid");
            entity.Property(e => e.Securitystamp)
                .HasMaxLength(128)
                .HasColumnName("securitystamp");
            entity.Property(e => e.Shouldchangepasswordonnextlogin).HasColumnName("shouldchangepasswordonnextlogin");
            entity.Property(e => e.Signatureimg).HasColumnName("signatureimg");
            entity.Property(e => e.Signintoken).HasColumnName("signintoken");
            entity.Property(e => e.Signintokenexpiretimeutc).HasColumnName("signintokenexpiretimeutc");
            entity.Property(e => e.Surname).HasColumnName("surname");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Unitid).HasColumnName("unitid");
            entity.Property(e => e.UseridQlvb)
                .HasMaxLength(50)
                .HasColumnName("userid_qlvb");
            entity.Property(e => e.Useridcchc)
                .HasMaxLength(50)
                .HasColumnName("useridcchc");
            entity.Property(e => e.Useridkcl).HasColumnName("useridkcl");
            entity.Property(e => e.Username)
                .HasMaxLength(500)
                .HasColumnName("username");
            entity.Property(e => e.Useruid).HasColumnName("useruid");
        });

        modelBuilder.Entity<Abpuserrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_abpuserroles");

            entity.ToTable("abpuserroles", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolelevel).HasColumnName("rolelevel");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_attachment");

            entity.ToTable("attachment", "dbo");

            entity.HasIndex(e => new { e.Objecttypeid, e.Objectid, e.Isdeleted }, "idx_ix_attachment_objecttypeid_objectid_isdeleted");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Authorid).HasColumnName("authorid");
            entity.Property(e => e.Contenttype).HasColumnName("contenttype");
            entity.Property(e => e.Convertdiskdirectory).HasColumnName("convertdiskdirectory");
            entity.Property(e => e.Convertfilename).HasColumnName("convertfilename");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Diskdirectory).HasColumnName("diskdirectory");
            entity.Property(e => e.Diskfilename).HasColumnName("diskfilename");
            entity.Property(e => e.Filename).HasColumnName("filename");
            entity.Property(e => e.Filesize).HasColumnName("filesize");
            entity.Property(e => e.Iscurrent).HasColumnName("iscurrent");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Objecttypeid).HasColumnName("objecttypeid");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<Attachmentsyntax>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_attachmentsyntax");

            entity.ToTable("attachmentsyntax", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Ischangesyntaxname)
                .HasDefaultValue(false)
                .HasColumnName("ischangesyntaxname");
            entity.Property(e => e.Isdefault)
                .HasDefaultValue(false)
                .HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Syntaxname).HasColumnName("syntaxname");
            entity.Property(e => e.Syntaxpath).HasColumnName("syntaxpath");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_category");

            entity.ToTable("category", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(50)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Typecode)
                .HasMaxLength(100)
                .HasColumnName("typecode");
            entity.Property(e => e.Typeid).HasColumnName("typeid");
        });

        modelBuilder.Entity<Categorydetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_categorydetail");

            entity.ToTable("categorydetail", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categorycode)
                .HasMaxLength(200)
                .HasColumnName("categorycode");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(50)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("pk_cities");

            entity.ToTable("cities", "dbo");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Citycode).HasColumnName("citycode");
            entity.Property(e => e.Cityname).HasColumnName("cityname");
            entity.Property(e => e.Indexid).HasColumnName("indexid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
        });

        modelBuilder.Entity<CtkmT10d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__ctkm_t10d__a4ae64b8304c97e3");

            entity.ToTable("ctkm_t10d", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Diemden).HasColumnName("diemden");
            entity.Property(e => e.Diemtu).HasColumnName("diemtu");
            entity.Property(e => e.Havevoucher)
                .HasDefaultValue(false)
                .HasColumnName("havevoucher");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CtkmT10dDe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__ctkm_t10d_de__a4ae64b8304c97e3");

            entity.ToTable("ctkm_t10d_de", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Kmcode)
                .HasMaxLength(50)
                .HasColumnName("kmcode");
            entity.Property(e => e.Kmid).HasColumnName("kmid");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Districtid).HasName("pk_districts");

            entity.ToTable("districts", "dbo");

            entity.Property(e => e.Districtid).HasColumnName("districtid");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Danso).HasColumnName("danso");
            entity.Property(e => e.Districtcode).HasColumnName("districtcode");
            entity.Property(e => e.Districtname).HasColumnName("districtname");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
        });

        modelBuilder.Entity<Drdisplay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_drdisplay");

            entity.ToTable("drdisplay", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Allowmergecells)
                .HasDefaultValue(false)
                .HasColumnName("allowmergecells");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Colnum).HasColumnName("colnum");
            entity.Property(e => e.Columnsetdata).HasColumnName("columnsetdata");
            entity.Property(e => e.Configheader).HasColumnName("configheader");
            entity.Property(e => e.Configpopup).HasColumnName("configpopup");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Cssheader).HasColumnName("cssheader");
            entity.Property(e => e.Customsummary).HasColumnName("customsummary");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Editcelltemplate).HasColumnName("editcelltemplate");
            entity.Property(e => e.Editcolumns).HasColumnName("editcolumns");
            entity.Property(e => e.Format).HasColumnName("format");
            entity.Property(e => e.Formulasyntax).HasColumnName("formulasyntax");
            entity.Property(e => e.Freepaneposition).HasColumnName("freepaneposition");
            entity.Property(e => e.Grouplevel).HasColumnName("grouplevel");
            entity.Property(e => e.Groupsort).HasColumnName("groupsort");
            entity.Property(e => e.Isavg)
                .HasDefaultValue(false)
                .HasColumnName("isavg");
            entity.Property(e => e.Iscount)
                .HasDefaultValue(false)
                .HasColumnName("iscount");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isdisplay).HasColumnName("isdisplay");
            entity.Property(e => e.Isexpand)
                .HasDefaultValue(false)
                .HasColumnName("isexpand");
            entity.Property(e => e.Isexport)
                .HasDefaultValue(false)
                .HasColumnName("isexport");
            entity.Property(e => e.Isfreepane).HasColumnName("isfreepane");
            entity.Property(e => e.Ismax)
                .HasDefaultValue(false)
                .HasColumnName("ismax");
            entity.Property(e => e.Ismin)
                .HasDefaultValue(false)
                .HasColumnName("ismin");
            entity.Property(e => e.Isparent).HasColumnName("isparent");
            entity.Property(e => e.Isreadonly)
                .HasDefaultValue(false)
                .HasColumnName("isreadonly");
            entity.Property(e => e.Issort)
                .HasDefaultValue(false)
                .HasColumnName("issort");
            entity.Property(e => e.Issum).HasColumnName("issum");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Parentcode).HasColumnName("parentcode");
            entity.Property(e => e.Pivotfield).HasColumnName("pivotfield");
            entity.Property(e => e.Pivotorders).HasColumnName("pivotorders");
            entity.Property(e => e.Reportcode)
                .HasMaxLength(200)
                .HasColumnName("reportcode");
            entity.Property(e => e.Reportid).HasColumnName("reportid");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Showingroupfooter)
                .HasDefaultValue(false)
                .HasColumnName("showingroupfooter");
            entity.Property(e => e.Sortbycolumn).HasColumnName("sortbycolumn");
            entity.Property(e => e.Summarydisplaymode).HasColumnName("summarydisplaymode");
            entity.Property(e => e.Textalign).HasColumnName("textalign");
            entity.Property(e => e.Textissum).HasColumnName("textissum");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Validationrule).HasColumnName("validationrule");
            entity.Property(e => e.Visible)
                .HasDefaultValue(true)
                .HasColumnName("visible");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.Migrationid).HasName("pk___efmigrationshistory");

            entity.ToTable("__efmigrationshistory", "dbo");

            entity.Property(e => e.Migrationid)
                .HasMaxLength(150)
                .HasColumnName("migrationid");
            entity.Property(e => e.Productversion)
                .HasMaxLength(32)
                .HasColumnName("productversion");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__genders__3214ec07bb181d81");

            entity.ToTable("genders", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Getattachment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("getattachment", "dbo");

            entity.Property(e => e.Authorid).HasColumnName("authorid");
            entity.Property(e => e.Contenttype).HasColumnName("contenttype");
            entity.Property(e => e.Convertdiskdirectory).HasColumnName("convertdiskdirectory");
            entity.Property(e => e.Convertfilename).HasColumnName("convertfilename");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Diskdirectory).HasColumnName("diskdirectory");
            entity.Property(e => e.Diskfilename).HasColumnName("diskfilename");
            entity.Property(e => e.Filename).HasColumnName("filename");
            entity.Property(e => e.Filesize).HasColumnName("filesize");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Iscurrent).HasColumnName("iscurrent");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Objecttypeid).HasColumnName("objecttypeid");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<HinCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hin_category", "dbo");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(50)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Typecode)
                .HasMaxLength(100)
                .HasColumnName("typecode");
            entity.Property(e => e.Typeid).HasColumnName("typeid");
        });

        modelBuilder.Entity<HinCategorydetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hin_categorydetail", "dbo");

            entity.Property(e => e.Categorycode)
                .HasMaxLength(200)
                .HasColumnName("categorycode");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(50)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
        });

        modelBuilder.Entity<HinCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hin_cities", "dbo");

            entity.Property(e => e.Citycode).HasColumnName("citycode");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Cityname).HasColumnName("cityname");
            entity.Property(e => e.Indexid).HasColumnName("indexid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
        });

        modelBuilder.Entity<HinDistrict>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hin_districts", "dbo");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Danso).HasColumnName("danso");
            entity.Property(e => e.Districtcode).HasColumnName("districtcode");
            entity.Property(e => e.Districtid).HasColumnName("districtid");
            entity.Property(e => e.Districtname).HasColumnName("districtname");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
        });

        modelBuilder.Entity<HinWard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hin_wards", "dbo");

            entity.Property(e => e.Districtid).HasColumnName("districtid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Wardcode).HasColumnName("wardcode");
            entity.Property(e => e.Wardid).HasColumnName("wardid");
            entity.Property(e => e.Wardname).HasColumnName("wardname");
        });

        modelBuilder.Entity<HsBooking>(entity =>
        {
            entity.HasKey(e => e.Bookingid).HasName("pk__hs_booki__73951acdeb4a6008");

            entity.ToTable("hs_booking", "dbo");

            entity.HasIndex(e => e.Checkindate, "idx_nonclusteredindex_checkindate");

            entity.HasIndex(e => e.Checkoutdate, "idx_nonclusteredindex_checkoutdate");

            entity.HasIndex(e => e.Customerid, "idx_nonclusteredindex_customerid");

            entity.HasIndex(e => e.Isactive, "idx_nonclusteredindex_isactive");

            entity.HasIndex(e => e.Isdeleted, "idx_nonclusteredindex_isdeleted");

            entity.HasIndex(e => e.Ispay, "idx_nonclusteredindex_ispay");

            entity.HasIndex(e => e.Roomid, "idx_nonclusteredindex_roomid");

            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Bookingdate).HasColumnName("bookingdate");
            entity.Property(e => e.Checkindate).HasColumnName("checkindate");
            entity.Property(e => e.Checkoutdate).HasColumnName("checkoutdate");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Deposit).HasColumnName("deposit");
            entity.Property(e => e.Discountpercent).HasColumnName("discountpercent");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Ispay)
                .HasDefaultValue(false)
                .HasColumnName("ispay");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Otherdiscountamount).HasColumnName("otherdiscountamount");
            entity.Property(e => e.Otherphonenumber)
                .HasMaxLength(20)
                .HasColumnName("otherphonenumber");
            entity.Property(e => e.Reasoncancel).HasColumnName("reasoncancel");
            entity.Property(e => e.Reasondiscount).HasColumnName("reasondiscount");
            entity.Property(e => e.Roomid).HasColumnName("roomid");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");
            entity.Property(e => e.Totaltime).HasColumnName("totaltime");
            entity.Property(e => e.Unitprice).HasColumnName("unitprice");

            entity.HasOne(d => d.Customer).WithMany(p => p.HsBookings)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk__hs_bookin__custo__2c578814");

            entity.HasOne(d => d.Room).WithMany(p => p.HsBookings)
                .HasForeignKey(d => d.Roomid)
                .HasConstraintName("fk__hs_bookin__roomi__2d4bac4d");
        });

        modelBuilder.Entity<HsBookingservice>(entity =>
        {
            entity.HasKey(e => e.Bookingserviceid).HasName("pk__hs_booki__43f55cd171b565df");

            entity.ToTable("hs_bookingservice", "dbo");

            entity.Property(e => e.Bookingserviceid).HasColumnName("bookingserviceid");
            entity.Property(e => e.Additionfromdate).HasColumnName("additionfromdate");
            entity.Property(e => e.Additiontodate).HasColumnName("additiontodate");
            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Totalprice).HasColumnName("totalprice");

            entity.HasOne(d => d.Booking).WithMany(p => p.HsBookingservices)
                .HasForeignKey(d => d.Bookingid)
                .HasConstraintName("fk__hs_bookin__booki__36d51687");

            entity.HasOne(d => d.Service).WithMany(p => p.HsBookingservices)
                .HasForeignKey(d => d.Serviceid)
                .HasConstraintName("fk__hs_bookin__servi__48e80e73");
        });

        modelBuilder.Entity<HsChiphi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__hs_chiph__3214ec07c7f42e9c");

            entity.ToTable("hs_chiphi", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .HasColumnName("content");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Expensedatetime).HasColumnName("expensedatetime");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Ischeck)
                .HasDefaultValue(false)
                .HasColumnName("ischeck");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Totalamount).HasColumnName("totalamount");
        });

        modelBuilder.Entity<HsCustomer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("pk__hs_custo__a4ae64b8304c97e3");

            entity.ToTable("hs_customer", "dbo");

            entity.HasIndex(e => e.Lastname, "idx_nonclusteredindex_hoten");

            entity.HasIndex(e => e.Phonenumber, "idx_nonclusteredindex_phonenumber");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .HasColumnName("cccd");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Mxh)
                .HasMaxLength(200)
                .HasColumnName("mxh");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<HsDichvutheolich>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hs_dichvutheolich", "dbo");

            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Listdichvu).HasColumnName("listdichvu");
        });

        modelBuilder.Entity<HsHomestay>(entity =>
        {
            entity.HasKey(e => e.Homestayid).HasName("pk__hs_homes__edcb5cda4c5dec80");

            entity.ToTable("hs_homestay", "dbo");

            entity.Property(e => e.Homestayid).HasColumnName("homestayid");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Ownerid).HasColumnName("ownerid");

            entity.HasOne(d => d.Owner).WithMany(p => p.HsHomestays)
                .HasForeignKey(d => d.Ownerid)
                .HasConstraintName("fk__hs_homest__owner__6aefe058");
        });

        modelBuilder.Entity<HsOwner>(entity =>
        {
            entity.HasKey(e => e.Ownerid).HasName("pk__hs_owner__81938598b243a403");

            entity.ToTable("hs_owner", "dbo");

            entity.Property(e => e.Ownerid).HasColumnName("ownerid");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
        });

        modelBuilder.Entity<HsPayment>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("pk__hs_payme__9b556a58fd157a61");

            entity.ToTable("hs_payment", "dbo");

            entity.Property(e => e.Paymentid).HasColumnName("paymentid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Paymentdate).HasColumnName("paymentdate");
            entity.Property(e => e.Paymentinformation)
                .HasMaxLength(100)
                .HasColumnName("paymentinformation");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(50)
                .HasColumnName("paymentmethod");

            entity.HasOne(d => d.Booking).WithMany(p => p.HsPayments)
                .HasForeignKey(d => d.Bookingid)
                .HasConstraintName("fk__hs_paymen__booki__4246c933");
        });

        modelBuilder.Entity<HsReview>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("pk__hs_revie__74bc79aea8c7fe8b");

            entity.ToTable("hs_review", "dbo");

            entity.Property(e => e.Reviewid).HasColumnName("reviewid");
            entity.Property(e => e.Comment)
                .HasMaxLength(1000)
                .HasColumnName("comment");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Reviewdate).HasColumnName("reviewdate");
            entity.Property(e => e.Roomid).HasColumnName("roomid");

            entity.HasOne(d => d.Customer).WithMany(p => p.HsReviews)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk__hs_review__custo__1844d718");

            entity.HasOne(d => d.Room).WithMany(p => p.HsReviews)
                .HasForeignKey(d => d.Roomid)
                .HasConstraintName("fk__hs_review__roomi__5535a963");
        });

        modelBuilder.Entity<HsRoom>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("pk__hs_room__328639198358319a");

            entity.ToTable("hs_room", "dbo");

            entity.Property(e => e.Roomid).HasColumnName("roomid");
            entity.Property(e => e.Badgeclass)
                .HasMaxLength(50)
                .HasColumnName("badgeclass");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .HasColumnName("color");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.Homestayid).HasColumnName("homestayid");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(100)
                .HasColumnName("number");
            entity.Property(e => e.Roomimage)
                .HasMaxLength(100)
                .HasColumnName("roomimage");

            entity.HasOne(d => d.Homestay).WithMany(p => p.HsRooms)
                .HasForeignKey(d => d.Homestayid)
                .HasConstraintName("fk__hs_room__homesta__6cd828ca");
        });

        modelBuilder.Entity<HsService>(entity =>
        {
            entity.HasKey(e => e.Serviceid).HasName("pk__hs_servi__c51bb0ea62a40f9b");

            entity.ToTable("hs_service", "dbo");

            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Applydate)
                .HasMaxLength(50)
                .HasColumnName("applydate");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Formula).HasColumnName("formula");
            entity.Property(e => e.Fromhour)
                .HasMaxLength(50)
                .HasColumnName("fromhour");
            entity.Property(e => e.Intimerange).HasColumnName("intimerange");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isaddon)
                .HasDefaultValue(false)
                .HasColumnName("isaddon");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Ishourservice)
                .HasDefaultValue(true)
                .HasColumnName("ishourservice");
            entity.Property(e => e.Ispricebyroom)
                .HasDefaultValue(false)
                .HasColumnName("ispricebyroom");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Serviceimage)
                .HasMaxLength(100)
                .HasColumnName("serviceimage");
            entity.Property(e => e.Tohour)
                .HasMaxLength(50)
                .HasColumnName("tohour");
        });

        modelBuilder.Entity<HsServiceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__hs_servi__3214ec07a38cb74e");

            entity.ToTable("hs_service_history", "dbo");

            entity.HasIndex(e => new { e.Tablename, e.Creationtime }, "idx_ix_hs_service_history_tablename_creationtime");

            entity.HasIndex(e => new { e.Tablename, e.Isdeleted }, "idx_ix_hs_service_history_tablename_isdeleted");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Information).HasColumnName("information");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Tablename)
                .HasMaxLength(100)
                .HasColumnName("tablename");
        });

        modelBuilder.Entity<HsServicepricebyroom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__hs_servi__3214ec0782b239f1");

            entity.ToTable("hs_servicepricebyroom", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Roomid).HasColumnName("roomid");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Serviceprice)
                .HasPrecision(18, 2)
                .HasColumnName("serviceprice");
        });

        modelBuilder.Entity<HsThongtindatView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hs_thongtindat_view", "dbo");

            entity.Property(e => e.Bookingid).HasColumnName("bookingid");
            entity.Property(e => e.Checkininfo).HasColumnName("checkininfo");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Roomid).HasColumnName("roomid");
        });

        modelBuilder.Entity<Nation>(entity =>
        {
            entity.HasKey(e => e.Nationid).HasName("pk_nations");

            entity.ToTable("nations", "dbo");

            entity.Property(e => e.Nationid).HasColumnName("nationid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Nationcode).HasColumnName("nationcode");
            entity.Property(e => e.Nationname).HasColumnName("nationname");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
        });

        modelBuilder.Entity<Nationality>(entity =>
        {
            entity.HasKey(e => e.Nationalityid).HasName("pk_nationalities");

            entity.ToTable("nationalities", "dbo");

            entity.Property(e => e.Nationalityid).HasColumnName("nationalityid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Nationalitycode).HasColumnName("nationalitycode");
            entity.Property(e => e.Nationalityname).HasColumnName("nationalityname");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
        });

        modelBuilder.Entity<Radioyesno>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("radioyesno", "dbo");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_refreshtoken");

            entity.ToTable("refreshtoken", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Expiredat).HasColumnName("expiredat");
            entity.Property(e => e.Isrevoked)
                .HasDefaultValue(false)
                .HasColumnName("isrevoked");
            entity.Property(e => e.Issuedat).HasColumnName("issuedat");
            entity.Property(e => e.Isused)
                .HasDefaultValue(false)
                .HasColumnName("isused");
            entity.Property(e => e.Jwtid).HasColumnName("jwtid");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Wardid).HasName("pk_wards");

            entity.ToTable("wards", "dbo");

            entity.HasIndex(e => e.Districtid, "idx_ix_wards_districtid");

            entity.Property(e => e.Wardid).HasColumnName("wardid");
            entity.Property(e => e.Districtid).HasColumnName("districtid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Wardcode).HasColumnName("wardcode");
            entity.Property(e => e.Wardname).HasColumnName("wardname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
