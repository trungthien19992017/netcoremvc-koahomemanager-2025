using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KOAHome.EntityFramework;

public partial class TttConfigContext : DbContext
{
    public TttConfigContext()
    {
    }

    public TttConfigContext(DbContextOptions<TttConfigContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NetAction> NetActions { get; set; }

    public virtual DbSet<NetActionlist> NetActionlists { get; set; }

    public virtual DbSet<NetActionlistdetail> NetActionlistdetails { get; set; }

    public virtual DbSet<NetActiontype> NetActiontypes { get; set; }

    public virtual DbSet<NetAttachment> NetAttachments { get; set; }

    public virtual DbSet<NetAttachmentsyntax> NetAttachmentsyntaxes { get; set; }

    public virtual DbSet<NetDashboard> NetDashboards { get; set; }

    public virtual DbSet<NetDashboardpage> NetDashboardpages { get; set; }

    public virtual DbSet<NetDatasource> NetDatasources { get; set; }

    public virtual DbSet<NetDatasourcedetail> NetDatasourcedetails { get; set; }

    public virtual DbSet<NetDisplay> NetDisplays { get; set; }

    public virtual DbSet<NetDynamicfield> NetDynamicfields { get; set; }

    public virtual DbSet<NetFilter> NetFilters { get; set; }

    public virtual DbSet<NetForm> NetForms { get; set; }

    public virtual DbSet<NetFormVersion> NetFormVersions { get; set; }

    public virtual DbSet<NetFormVersionfield> NetFormVersionfields { get; set; }

    public virtual DbSet<NetFormfieldtype> NetFormfieldtypes { get; set; }

    public virtual DbSet<NetMainmenu> NetMainmenus { get; set; }

    public virtual DbSet<NetMenu> NetMenus { get; set; }

    public virtual DbSet<NetMenurole> NetMenuroles { get; set; }

    public virtual DbSet<NetReport> NetReports { get; set; }

    public virtual DbSet<NetService> NetServices { get; set; }

    public virtual DbSet<NetTabpanel> NetTabpanels { get; set; }

    public virtual DbSet<NetTabpanelDetail> NetTabpanelDetails { get; set; }

    public virtual DbSet<NetValidation> NetValidations { get; set; }

    public virtual DbSet<NetWidget> NetWidgets { get; set; }

    public virtual DbSet<NetWidgetdefaultconfig> NetWidgetdefaultconfigs { get; set; }

    public virtual DbSet<NetWidgetgroup> NetWidgetgroups { get; set; }

    public virtual DbSet<NetWidgetitem> NetWidgetitems { get; set; }

    public virtual DbSet<NetWidgetmap> NetWidgetmaps { get; set; }

    public virtual DbSet<NetWidgetvalueconfig> NetWidgetvalueconfigs { get; set; }

    public virtual DbSet<Tempqueriescopy> Tempqueriescopies { get; set; }

    public virtual DbSet<Tempquery> Tempqueries { get; set; }

    public virtual DbSet<WidgetlayoutTest> WidgetlayoutTests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:ConfigConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_action");

            entity.ToTable("net_action", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Confirmbuttontext)
                .HasMaxLength(10)
                .HasColumnName("confirmbuttontext");
            entity.Property(e => e.Confirmtext)
                .HasMaxLength(50)
                .HasColumnName("confirmtext");
            entity.Property(e => e.Confirmtitle)
                .HasMaxLength(10)
                .HasColumnName("confirmtitle");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Ispopupconfirm).HasColumnName("ispopupconfirm");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
        });

        modelBuilder.Entity<NetActionlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_actionlist");

            entity.ToTable("net_actionlist", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actionlisttypecode)
                .HasMaxLength(200)
                .HasColumnName("actionlisttypecode");
            entity.Property(e => e.Actionlisttypeid).HasColumnName("actionlisttypeid");
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
            entity.Property(e => e.Objectcode)
                .HasMaxLength(200)
                .HasColumnName("objectcode");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(50)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
        });

        modelBuilder.Entity<NetActionlistdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_actionlistdetail");

            entity.ToTable("net_actionlistdetail", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actionid).HasColumnName("actionid");
            entity.Property(e => e.Actionlistcode).HasColumnName("actionlistcode");
            entity.Property(e => e.Actionlistid).HasColumnName("actionlistid");
            entity.Property(e => e.Checksamepopupbutton)
                .HasMaxLength(20)
                .HasColumnName("checksamepopupbutton");
            entity.Property(e => e.Checksamepopuptext).HasColumnName("checksamepopuptext");
            entity.Property(e => e.Codesendrealtime).HasColumnName("codesendrealtime");
            entity.Property(e => e.Confirmbuttontext).HasColumnName("confirmbuttontext");
            entity.Property(e => e.Confirmtext).HasColumnName("confirmtext");
            entity.Property(e => e.Confirmtitle).HasColumnName("confirmtitle");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Cssbutton).HasColumnName("cssbutton");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteuserid).HasColumnName("deleteuserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Displayname).HasColumnName("displayname");
            entity.Property(e => e.Errorcol).HasColumnName("errorcol");
            entity.Property(e => e.Filetemplate).HasColumnName("filetemplate");
            entity.Property(e => e.Filetypeaccept)
                .HasMaxLength(50)
                .HasColumnName("filetypeaccept");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .HasColumnName("icon");
            entity.Property(e => e.Idgroup)
                .HasMaxLength(50)
                .HasColumnName("idgroup");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Ischecksamepopup).HasColumnName("ischecksamepopup");
            entity.Property(e => e.Ischoosedata).HasColumnName("ischoosedata");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isgroup).HasColumnName("isgroup");
            entity.Property(e => e.IsnetActionhowerror).HasColumnName("isnet_actionhowerror");
            entity.Property(e => e.Ispopupconfirm).HasColumnName("ispopupconfirm");
            entity.Property(e => e.Issendrealtime)
                .HasDefaultValue(false)
                .HasColumnName("issendrealtime");
            entity.Property(e => e.Istop).HasColumnName("istop");
            entity.Property(e => e.Iszoompopup)
                .HasDefaultValue(false)
                .HasColumnName("iszoompopup");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Servicefilename).HasColumnName("servicefilename");
            entity.Property(e => e.Sitecode).HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Typenodediagram)
                .HasMaxLength(100)
                .HasColumnName("typenodediagram");
            entity.Property(e => e.Urlimportfile).HasColumnName("urlimportfile");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<NetActiontype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_actiontype");

            entity.ToTable("net_actiontype", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
        });

        modelBuilder.Entity<NetAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_attachment");

            entity.ToTable("net_attachment", "dbo");

            entity.HasIndex(e => new { e.Objecttypeid, e.Objectid, e.Isdeleted }, "idx_ix_net_attachment_objecttypeid_objectid_isdeleted");

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
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<NetAttachmentsyntax>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_attachmentsyntax");

            entity.ToTable("net_attachmentsyntax", "dbo");

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

        modelBuilder.Entity<NetDashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_hin_dashboards");

            entity.ToTable("net_dashboard", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Autoreload)
                .HasDefaultValue(false)
                .HasColumnName("autoreload");
            entity.Property(e => e.Codereceiverealtime).HasColumnName("codereceiverealtime");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Dashboardcode).HasColumnName("dashboardcode");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.Showcalendarfilter)
                .HasDefaultValue(false)
                .HasColumnName("showcalendarfilter");
            entity.Property(e => e.Storedefault).HasColumnName("storedefault");
            entity.Property(e => e.Textcalendarcolor).HasColumnName("textcalendarcolor");
        });

        modelBuilder.Entity<NetDashboardpage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_dashboardpage");

            entity.ToTable("net_dashboardpage", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Dashboardid).HasColumnName("dashboardid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Pagecode).HasColumnName("pagecode");
        });

        modelBuilder.Entity<NetDatasource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_datasource");

            entity.ToTable("net_datasource", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Sqltype).HasColumnName("sqltype");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<NetDatasourcedetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_datasourcedetail");

            entity.ToTable("net_datasourcedetail", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Configid).HasColumnName("configid");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Dbname).HasColumnName("dbname");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Host).HasColumnName("host");
            entity.Property(e => e.Isdefault).HasColumnName("isdefault");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Timeout).HasColumnName("timeout");
            entity.Property(e => e.User).HasColumnName("user");
        });

        modelBuilder.Entity<NetDisplay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_display");

            entity.ToTable("net_display", "dbo");

            entity.HasIndex(e => e.Reportid, "idx_ix_missingindex_61_60");

            entity.HasIndex(e => new { e.Isdeleted, e.Reportid }, "idx_ix_missingindex_64_63");

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

        modelBuilder.Entity<NetDynamicfield>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_dynamicfield");

            entity.ToTable("net_dynamicfield", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<NetFilter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_filter");

            entity.ToTable("net_filter", "dbo");

            entity.HasIndex(e => new { e.Isdeleted, e.Reportid, e.Isactive }, "idx_ix_missingindex_12_11");

            entity.HasIndex(e => new { e.Isdeleted, e.Reportid, e.Dynamicfieldid, e.Isactive }, "idx_ix_missingindex_536_535");

            entity.HasIndex(e => new { e.Reportid, e.Dynamicfieldid }, "idx_ix_missingindex_568_567");

            entity.HasIndex(e => new { e.Isdeleted, e.Reportid }, "idx_ix_missingindex_6_5");

            entity.HasIndex(e => new { e.Isdeleted, e.Reportid }, "idx_ix_missingindex_8_7");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Colcount).HasColumnName("colcount");
            entity.Property(e => e.Colspan).HasColumnName("colspan");
            entity.Property(e => e.Columns).HasColumnName("columns");
            entity.Property(e => e.Combolevel).HasColumnName("combolevel");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datatype).HasColumnName("datatype");
            entity.Property(e => e.Datedisplayformat)
                .HasMaxLength(50)
                .HasColumnName("datedisplayformat");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Disable).HasColumnName("disable");
            entity.Property(e => e.Dynamicfieldid).HasColumnName("dynamicfieldid");
            entity.Property(e => e.Format).HasColumnName("format");
            entity.Property(e => e.Groupfield)
                .HasMaxLength(50)
                .HasColumnName("groupfield");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isfiltertoolbar)
                .HasDefaultValue(false)
                .HasColumnName("isfiltertoolbar");
            entity.Property(e => e.Isgrouped)
                .HasDefaultValue(false)
                .HasColumnName("isgrouped");
            entity.Property(e => e.Isloadmultipleway).HasColumnName("isloadmultipleway");
            entity.Property(e => e.Isvalue).HasColumnName("isvalue");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Lookupid).HasColumnName("lookupid");
            entity.Property(e => e.Multicontrolid).HasColumnName("multicontrolid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Parentcomboid).HasColumnName("parentcomboid");
            entity.Property(e => e.Reportcode)
                .HasMaxLength(200)
                .HasColumnName("reportcode");
            entity.Property(e => e.Reportid).HasColumnName("reportid");
            entity.Property(e => e.Required).HasColumnName("required");
            entity.Property(e => e.Seviceid).HasColumnName("seviceid");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.Width).HasColumnName("width");
            entity.Property(e => e.Zoomlevel)
                .HasMaxLength(50)
                .HasColumnName("zoomlevel");
        });

        modelBuilder.Entity<NetForm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_form");

            entity.ToTable("net_form", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Codereceiverealtime).HasColumnName("codereceiverealtime");
            entity.Property(e => e.Codesendrealtime).HasColumnName("codesendrealtime");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Cssoptionheader).HasColumnName("cssoptionheader");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isreceiverealtime)
                .HasDefaultValue(false)
                .HasColumnName("isreceiverealtime");
            entity.Property(e => e.Issendrealtime)
                .HasDefaultValue(false)
                .HasColumnName("issendrealtime");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Latestversion).HasColumnName("latestversion");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<NetFormVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_form_version");

            entity.ToTable("net_form_version", "dbo");

            entity.HasIndex(e => new { e.Isdeleted, e.Hinformid, e.Isactive }, "idx_ix_missingindex_18_17");

            entity.HasIndex(e => e.Hinformid, "idx_ix_missingindex_207_206");

            entity.HasIndex(e => new { e.Isdeleted, e.Hinformid, e.Isactive }, "idx_ix_missingindex_35_34");

            entity.HasIndex(e => new { e.Isdeleted, e.Hinformid, e.Isactive }, "idx_ix_missingindex_42_41");

            entity.HasIndex(e => new { e.Isdeleted, e.Hinformid, e.Isactive }, "idx_ix_missingindex_55_54");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apicontent).HasColumnName("apicontent");
            entity.Property(e => e.Conditionofaction).HasColumnName("conditionofaction");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Exportmergefield)
                .HasDefaultValue(false)
                .HasColumnName("exportmergefield");
            entity.Property(e => e.Hinformbookvalueid).HasColumnName("hinformbookvalueid");
            entity.Property(e => e.Hinformcode)
                .HasMaxLength(200)
                .HasColumnName("hinformcode");
            entity.Property(e => e.Hinformid).HasColumnName("hinformid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isback).HasColumnName("isback");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isview).HasColumnName("isview");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Objectcode).HasColumnName("objectcode");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Positionbutton).HasColumnName("positionbutton");
            entity.Property(e => e.Saveeditortype).HasColumnName("saveeditortype");
            entity.Property(e => e.Storecheckurl).HasColumnName("storecheckurl");
            entity.Property(e => e.Storedefaultdata).HasColumnName("storedefaultdata");
            entity.Property(e => e.Storegetdata).HasColumnName("storegetdata");
            entity.Property(e => e.Storelabelaction).HasColumnName("storelabelaction");
            entity.Property(e => e.Storesetdata).HasColumnName("storesetdata");
            entity.Property(e => e.Storesetreadonly).HasColumnName("storesetreadonly");
            entity.Property(e => e.Tablename).HasColumnName("tablename");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Version).HasColumnName("version");
        });

        modelBuilder.Entity<NetFormVersionfield>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_form_versionfield");

            entity.ToTable("net_form_versionfield", "dbo");

            entity.HasIndex(e => new { e.Hinformversionid, e.Isactive }, "idx_ix_missingindex_209_208");

            entity.HasIndex(e => new { e.Isdeleted, e.Hinformversionid, e.Isactive }, "idx_ix_missingindex_86_85");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasources).HasColumnName("datasources");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Fieldtypeid).HasColumnName("fieldtypeid");
            entity.Property(e => e.Hinformcode)
                .HasMaxLength(200)
                .HasColumnName("hinformcode");
            entity.Property(e => e.Hinformversionid).HasColumnName("hinformversionid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Parentcode).HasColumnName("parentcode");
            entity.Property(e => e.Parentid).HasColumnName("parentid");
            entity.Property(e => e.Tabindex).HasColumnName("tabindex");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.Validates).HasColumnName("validates");
        });

        modelBuilder.Entity<NetFormfieldtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_formfieldtype");

            entity.ToTable("net_formfieldtype", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isrowtemplate)
                .HasDefaultValue(false)
                .HasColumnName("isrowtemplate");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
        });

        modelBuilder.Entity<NetMainmenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_mainmenu");

            entity.ToTable("net_mainmenu", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Devicetype).HasColumnName("devicetype");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isminiitem)
                .HasDefaultValue(false)
                .HasColumnName("isminiitem");
            entity.Property(e => e.Ismobile).HasColumnName("ismobile");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Parent).HasColumnName("parent");
            entity.Property(e => e.Requiredpermissionname).HasColumnName("requiredpermissionname");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<NetMenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_menu");

            entity.ToTable("net_menu", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Countnum).HasColumnName("countnum");
            entity.Property(e => e.Countoutofdate).HasColumnName("countoutofdate");
            entity.Property(e => e.Counttype).HasColumnName("counttype");
            entity.Property(e => e.Cssformat).HasColumnName("cssformat");
            entity.Property(e => e.Cssiconformat).HasColumnName("cssiconformat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Iframe).HasColumnName("iframe");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Iscount).HasColumnName("iscount");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValue(false)
                .HasColumnName("isdeleted");
            entity.Property(e => e.Israwsql).HasColumnName("israwsql");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.Menuid).HasColumnName("menuid");
            entity.Property(e => e.Mobilelink).HasColumnName("mobilelink");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Organizationid).HasColumnName("organizationid");
            entity.Property(e => e.Parent).HasColumnName("parent");
            entity.Property(e => e.Parentorgid).HasColumnName("parentorgid");
            entity.Property(e => e.Requiredpermissionname).HasColumnName("requiredpermissionname");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Sqlcountstore).HasColumnName("sqlcountstore");
            entity.Property(e => e.Sqlstring).HasColumnName("sqlstring");
            entity.Property(e => e.Textcolor).HasColumnName("textcolor");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Typecheck).HasColumnName("typecheck");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<NetMenurole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_menurole");

            entity.ToTable("net_menurole", "dbo");

            entity.HasIndex(e => e.Tenantid, "idx_ix_net_menurole_tenantid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Labelid).HasColumnName("labelid");
            entity.Property(e => e.Menuid).HasColumnName("menuid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolemappergroupid).HasColumnName("rolemappergroupid");
            entity.Property(e => e.Tenantid).HasColumnName("tenantid");
        });

        modelBuilder.Entity<NetReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_report");

            entity.ToTable("net_report", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Allowedapi).HasColumnName("allowedapi");
            entity.Property(e => e.Allowedpagesizes).HasColumnName("allowedpagesizes");
            entity.Property(e => e.Cache)
                .HasDefaultValue(false)
                .HasColumnName("cache");
            entity.Property(e => e.Chartviewdisplay).HasColumnName("chartviewdisplay");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Colcount).HasColumnName("colcount");
            entity.Property(e => e.Colspan).HasColumnName("colspan");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Defaultparam).HasColumnName("defaultparam");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Disablehandlecollumn).HasColumnName("disablehandlecollumn");
            entity.Property(e => e.Disablesearch)
                .HasDefaultValue(false)
                .HasColumnName("disablesearch");
            entity.Property(e => e.Displaytype).HasColumnName("displaytype");
            entity.Property(e => e.Editingmode).HasColumnName("editingmode");
            entity.Property(e => e.Enablemasterdetail)
                .HasDefaultValue(false)
                .HasColumnName("enablemasterdetail");
            entity.Property(e => e.Excel).HasColumnName("excel");
            entity.Property(e => e.Formid).HasColumnName("formid");
            entity.Property(e => e.Functioncode)
                .HasMaxLength(100)
                .HasColumnName("functioncode");
            entity.Property(e => e.Funtionid).HasColumnName("funtionid");
            entity.Property(e => e.Grouplevel).HasColumnName("grouplevel");
            entity.Property(e => e.Isautocollapse)
                .HasDefaultValue(false)
                .HasColumnName("isautocollapse");
            entity.Property(e => e.Isbackviewer)
                .HasDefaultValue(false)
                .HasColumnName("isbackviewer");
            entity.Property(e => e.Isbtnhandle)
                .HasDefaultValue(true)
                .HasColumnName("isbtnhandle");
            entity.Property(e => e.Iscreateeditor)
                .HasDefaultValue(true)
                .HasColumnName("iscreateeditor");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isdeleteeditor)
                .HasDefaultValue(false)
                .HasColumnName("isdeleteeditor");
            entity.Property(e => e.Isdynamiccolumn).HasColumnName("isdynamiccolumn");
            entity.Property(e => e.Isediteditor)
                .HasDefaultValue(true)
                .HasColumnName("isediteditor");
            entity.Property(e => e.Isexportexcel)
                .HasDefaultValue(true)
                .HasColumnName("isexportexcel");
            entity.Property(e => e.Isexportword).HasColumnName("isexportword");
            entity.Property(e => e.Isfreepane)
                .HasDefaultValue(true)
                .HasColumnName("isfreepane");
            entity.Property(e => e.Isrecieverealtime)
                .HasDefaultValue(false)
                .HasColumnName("isrecieverealtime");
            entity.Property(e => e.Issearchbar)
                .HasDefaultValue(true)
                .HasColumnName("issearchbar");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Layoutpfilter).HasColumnName("layoutpfilter");
            entity.Property(e => e.Masterdetailreportcode).HasColumnName("masterdetailreportcode");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Pagination)
                .HasDefaultValue(false)
                .HasColumnName("pagination");
            entity.Property(e => e.Positionbutton).HasColumnName("positionbutton");
            entity.Property(e => e.Reportcoderecieverealtime).HasColumnName("reportcoderecieverealtime");
            entity.Property(e => e.Reporttype).HasColumnName("reporttype");
            entity.Property(e => e.Selectiontype)
                .HasMaxLength(10)
                .HasColumnName("selectiontype");
            entity.Property(e => e.Servicehiddenfilter).HasColumnName("servicehiddenfilter");
            entity.Property(e => e.Showheaderfilter)
                .HasDefaultValue(false)
                .HasColumnName("showheaderfilter");
            entity.Property(e => e.Showiconfilter)
                .HasDefaultValue(false)
                .HasColumnName("showiconfilter");
            entity.Property(e => e.Showpage)
                .HasDefaultValue(true)
                .HasColumnName("showpage");
            entity.Property(e => e.Showtoolbar)
                .HasDefaultValue(true)
                .HasColumnName("showtoolbar");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Sqlcontent).HasColumnName("sqlcontent");
            entity.Property(e => e.Sqlcontentm).HasColumnName("sqlcontentm");
            entity.Property(e => e.Sqldefaultcontent).HasColumnName("sqldefaultcontent");
            entity.Property(e => e.Sqleditcontent).HasColumnName("sqleditcontent");
            entity.Property(e => e.Sqledittemplatecontent).HasColumnName("sqledittemplatecontent");
            entity.Property(e => e.Sqlexportdata).HasColumnName("sqlexportdata");
            entity.Property(e => e.Sqlexportfield).HasColumnName("sqlexportfield");
            entity.Property(e => e.Sqlstoredlabelaction).HasColumnName("sqlstoredlabelaction");
            entity.Property(e => e.Sqltype).HasColumnName("sqltype");
            entity.Property(e => e.Sqltypem)
                .HasDefaultValue(false)
                .HasColumnName("sqltypem");
            entity.Property(e => e.Storecheckurl).HasColumnName("storecheckurl");
            entity.Property(e => e.Storedrag).HasColumnName("storedrag");
            entity.Property(e => e.Storedrdisplay).HasColumnName("storedrdisplay");
            entity.Property(e => e.Templateids).HasColumnName("templateids");
            entity.Property(e => e.Typegetcolumn).HasColumnName("typegetcolumn");
            entity.Property(e => e.Word).HasColumnName("word");
        });

        modelBuilder.Entity<NetService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_service");

            entity.ToTable("net_service", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cache)
                .HasDefaultValue(true)
                .HasColumnName("cache");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Coldisplay).HasColumnName("coldisplay");
            entity.Property(e => e.Colparent).HasColumnName("colparent");
            entity.Property(e => e.Colvalue).HasColumnName("colvalue");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Sqlcontent).HasColumnName("sqlcontent");
            entity.Property(e => e.Sqltype).HasColumnName("sqltype");
            entity.Property(e => e.Storeddefaultparam).HasColumnName("storeddefaultparam");
        });

        modelBuilder.Entity<NetTabpanel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_tabpanel");

            entity.ToTable("net_tabpanel", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Aftereffecticon)
                .HasMaxLength(100)
                .HasColumnName("aftereffecticon");
            entity.Property(e => e.Aftereffecticoncolor)
                .HasMaxLength(100)
                .HasColumnName("aftereffecticoncolor");
            entity.Property(e => e.Beforeeffecticon)
                .HasMaxLength(100)
                .HasColumnName("beforeeffecticon");
            entity.Property(e => e.Beforeeffecticoncolor)
                .HasMaxLength(100)
                .HasColumnName("beforeeffecticoncolor");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Filetemplate).HasColumnName("filetemplate");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Iseffecticon).HasColumnName("iseffecticon");
            entity.Property(e => e.Isexportexcel).HasColumnName("isexportexcel");
            entity.Property(e => e.Isexportwordtemplate).HasColumnName("isexportwordtemplate");
            entity.Property(e => e.Ispermission).HasColumnName("ispermission");
            entity.Property(e => e.Ispermissionbyrecord).HasColumnName("ispermissionbyrecord");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Selectedindex).HasColumnName("selectedindex");
            entity.Property(e => e.Servicegetfilename).HasColumnName("servicegetfilename");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Storechecktabdetail)
                .HasMaxLength(100)
                .HasColumnName("storechecktabdetail");
            entity.Property(e => e.Storecheckurl).HasColumnName("storecheckurl");
            entity.Property(e => e.Storecountnotify).HasColumnName("storecountnotify");
            entity.Property(e => e.Storeexportfile).HasColumnName("storeexportfile");
            entity.Property(e => e.Storegetdata).HasColumnName("storegetdata");
            entity.Property(e => e.Storegetfieldexportdatagrid).HasColumnName("storegetfieldexportdatagrid");
            entity.Property(e => e.Storegetfieldexportform).HasColumnName("storegetfieldexportform");
            entity.Property(e => e.Storepermissionbyrecord)
                .HasMaxLength(100)
                .HasColumnName("storepermissionbyrecord");
            entity.Property(e => e.Storetabpermission).HasColumnName("storetabpermission");
        });

        modelBuilder.Entity<NetTabpanelDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_tabpanel_detail");

            entity.ToTable("net_tabpanel_detail", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Hintabpanelcode)
                .HasMaxLength(100)
                .HasColumnName("hintabpanelcode");
            entity.Property(e => e.Hintabpanelid).HasColumnName("hintabpanelid");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isloop)
                .HasDefaultValue(false)
                .HasColumnName("isloop");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Options).HasColumnName("options");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Storeloop).HasColumnName("storeloop");
            entity.Property(e => e.Tabicon)
                .HasMaxLength(50)
                .HasColumnName("tabicon");
            entity.Property(e => e.Template).HasColumnName("template");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<NetValidation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__drvalida__3214ec07c8d7f405");

            entity.ToTable("net_validation", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .HasColumnName("code");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .HasColumnName("message");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Pattern)
                .HasMaxLength(50)
                .HasColumnName("pattern");
            entity.Property(e => e.Sitecode)
                .HasMaxLength(100)
                .HasColumnName("sitecode");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Store)
                .HasMaxLength(100)
                .HasColumnName("store");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<NetWidget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widget");

            entity.ToTable("net_widget", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Displaytypecode).HasColumnName("displaytypecode");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Widgetcode).HasColumnName("widgetcode");
        });

        modelBuilder.Entity<NetWidgetdefaultconfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widgetdefaultconfig");

            entity.ToTable("net_widgetdefaultconfig", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Defaultvalue).HasColumnName("defaultvalue");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Indexnumber).HasColumnName("indexnumber");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Widgetlayoutid).HasColumnName("widgetlayoutid");
        });

        modelBuilder.Entity<NetWidgetgroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widgetgroup");

            entity.ToTable("net_widgetgroup", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Groupname).HasColumnName("groupname");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
        });

        modelBuilder.Entity<NetWidgetitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widgetitem");

            entity.ToTable("net_widgetitem", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Datasourceid).HasColumnName("datasourceid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Groupwidgetid).HasColumnName("groupwidgetid");
            entity.Property(e => e.Imgreview).HasColumnName("imgreview");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Templateids).HasColumnName("templateids");
            entity.Property(e => e.Widgetlayoutid).HasColumnName("widgetlayoutid");
        });

        modelBuilder.Entity<NetWidgetmap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widgetmap");

            entity.ToTable("net_widgetmap", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Creationtime).HasColumnName("creationtime");
            entity.Property(e => e.Creatoruserid).HasColumnName("creatoruserid");
            entity.Property(e => e.Deleteruserid).HasColumnName("deleteruserid");
            entity.Property(e => e.Deletiontime).HasColumnName("deletiontime");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Indexnumber).HasColumnName("indexnumber");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastmodificationtime).HasColumnName("lastmodificationtime");
            entity.Property(e => e.Lastmodifieruserid).HasColumnName("lastmodifieruserid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Pageid).HasColumnName("pageid");
            entity.Property(e => e.Positionx).HasColumnName("positionx");
            entity.Property(e => e.Positiony).HasColumnName("positiony");
            entity.Property(e => e.Widgetitemid).HasColumnName("widgetitemid");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<NetWidgetvalueconfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_widgetvalueconfig");

            entity.ToTable("net_widgetvalueconfig", "dbo");

            entity.HasIndex(e => new { e.Widgetitemid, e.Isdelete }, "idx_ix_missingindex_4_3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descriptions).HasColumnName("descriptions");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Isdelete).HasColumnName("isdelete");
            entity.Property(e => e.Key).HasColumnName("key");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Widgetitemid).HasColumnName("widgetitemid");
        });

        modelBuilder.Entity<Tempqueriescopy>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tempqueriescopy", "dbo");

            entity.Property(e => e.Col1)
                .HasMaxLength(376)
                .HasColumnName("col1");
            entity.Property(e => e.Col2).HasColumnName("col2");
            entity.Property(e => e.Col3)
                .HasMaxLength(259)
                .HasColumnName("col3");
            entity.Property(e => e.Rownum).HasColumnName("rownum");
        });

        modelBuilder.Entity<Tempquery>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tempqueries", "dbo");

            entity.Property(e => e.Col1)
                .HasMaxLength(376)
                .HasColumnName("col1");
            entity.Property(e => e.Col2).HasColumnName("col2");
            entity.Property(e => e.Col3)
                .HasMaxLength(259)
                .HasColumnName("col3");
            entity.Property(e => e.Rownum).HasColumnName("rownum");
        });

        modelBuilder.Entity<WidgetlayoutTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk__widgetla__3214ec07b34c3338");

            entity.ToTable("widgetlayout_test", "dbo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Dashboardid)
                .HasMaxLength(50)
                .HasColumnName("dashboardid");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.Widgetid)
                .HasMaxLength(100)
                .HasColumnName("widgetid");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
