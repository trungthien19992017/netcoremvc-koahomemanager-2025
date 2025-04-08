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

    public virtual DbSet<NetActionList> NetActionLists { get; set; }

    public virtual DbSet<NetActionListDetail> NetActionListDetails { get; set; }

    public virtual DbSet<NetAttachment> NetAttachments { get; set; }

    public virtual DbSet<NetAttachmentSyntax> NetAttachmentSyntaxes { get; set; }

    public virtual DbSet<NetDashboard> NetDashboards { get; set; }

    public virtual DbSet<NetDashboardPage> NetDashboardPages { get; set; }

    public virtual DbSet<NetDataSource> NetDataSources { get; set; }

    public virtual DbSet<NetDataSourceDetail> NetDataSourceDetails { get; set; }

    public virtual DbSet<NetDisplay> NetDisplays { get; set; }

    public virtual DbSet<NetFilter> NetFilters { get; set; }

    public virtual DbSet<NetForm> NetForms { get; set; }

    public virtual DbSet<NetFormVersion> NetFormVersions { get; set; }

    public virtual DbSet<NetFormVersionField> NetFormVersionFields { get; set; }

    public virtual DbSet<NetMenu> NetMenus { get; set; }

    public virtual DbSet<NetMenuRole> NetMenuRoles { get; set; }

    public virtual DbSet<NetReport> NetReports { get; set; }

    public virtual DbSet<NetService> NetServices { get; set; }

    public virtual DbSet<NetTabPanel> NetTabPanels { get; set; }

    public virtual DbSet<NetTabPanelDetail> NetTabPanelDetails { get; set; }

    public virtual DbSet<NetValidation> NetValidations { get; set; }

    public virtual DbSet<NetWidget> NetWidgets { get; set; }

    public virtual DbSet<NetWidgetDefaultConfig> NetWidgetDefaultConfigs { get; set; }

    public virtual DbSet<NetWidgetGroup> NetWidgetGroups { get; set; }

    public virtual DbSet<NetWidgetItem> NetWidgetItems { get; set; }

    public virtual DbSet<NetWidgetMap> NetWidgetMaps { get; set; }

    public virtual DbSet<NetWidgetValueConfig> NetWidgetValueConfigs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ConfigConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetAction>(entity =>
        {
            entity.ToTable("NET_Action");

            entity.Property(e => e.ConfirmButtonText).HasMaxLength(10);
            entity.Property(e => e.ConfirmText).HasMaxLength(50);
            entity.Property(e => e.ConfirmTitle).HasMaxLength(10);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetActionList>(entity =>
        {
            entity.ToTable("NET_ActionList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionListTypeCode).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.ObjectCode).HasMaxLength(200);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
        });

        modelBuilder.Entity<NetActionListDetail>(entity =>
        {
            entity.ToTable("NET_ActionListDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CheckSamePopupButton).HasMaxLength(20);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DataSourceId).HasColumnName("DataSourceID");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.FileTypeAccept).HasMaxLength(50);
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Idgroup)
                .HasMaxLength(50)
                .HasColumnName("IDGroup");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsNetActionhowError).HasColumnName("IsNET_ActionhowError");
            entity.Property(e => e.IsSendRealTime).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.RoleId).IsUnicode(false);
            entity.Property(e => e.SiteCode).IsUnicode(false);
            entity.Property(e => e.TypeNodeDiagram)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetAttachment>(entity =>
        {
            entity.ToTable("NET_Attachment");

            entity.HasIndex(e => new { e.ObjectTypeId, e.ObjectId, e.IsDeleted }, "IX_NET_Attachment_ObjectTypeId_ObjectId_IsDeleted");
        });

        modelBuilder.Entity<NetAttachmentSyntax>(entity =>
        {
            entity.ToTable("NET_AttachmentSyntax");

            entity.Property(e => e.IsChangeSyntaxName).HasDefaultValue(false);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
        });

        modelBuilder.Entity<NetDashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HIN_Dashboards");

            entity.ToTable("NET_Dashboard");
        });

        modelBuilder.Entity<NetDashboardPage>(entity =>
        {
            entity.ToTable("NET_DashboardPage");
        });

        modelBuilder.Entity<NetDataSource>(entity =>
        {
            entity.ToTable("NET_DataSource");

            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetDataSourceDetail>(entity =>
        {
            entity.ToTable("NET_DataSourceDetail");

            entity.Property(e => e.Dbname).HasColumnName("DBName");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetDisplay>(entity =>
        {
            entity.ToTable("NET_Display");

            entity.HasIndex(e => e.ReportId, "IX_MissingIndex_61_60").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId }, "IX_MissingIndex_64_63").HasFillFactor(90);

            entity.Property(e => e.CustomSummary).IsUnicode(false);
            entity.Property(e => e.FormulaSyntax).IsUnicode(false);
            entity.Property(e => e.IsAvg).HasDefaultValue(false);
            entity.Property(e => e.IsCount).HasDefaultValue(false);
            entity.Property(e => e.IsExpand).HasDefaultValue(false);
            entity.Property(e => e.IsMax).HasDefaultValue(false);
            entity.Property(e => e.IsMin).HasDefaultValue(false);
            entity.Property(e => e.IsSort).HasDefaultValue(false);
            entity.Property(e => e.ReportCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ShowInGroupFooter).HasDefaultValue(false);
            entity.Property(e => e.Visible).HasDefaultValue(true);
        });

        modelBuilder.Entity<NetFilter>(entity =>
        {
            entity.ToTable("NET_Filter");

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId, e.IsActive }, "IX_MissingIndex_12_11").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId, e.DynamicFieldId, e.IsActive }, "IX_MissingIndex_536_535").HasFillFactor(90);

            entity.HasIndex(e => new { e.ReportId, e.DynamicFieldId }, "IX_MissingIndex_568_567").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId }, "IX_MissingIndex_6_5").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId }, "IX_MissingIndex_8_7").HasFillFactor(90);

            entity.Property(e => e.DateDisplayFormat).HasMaxLength(50);
            entity.Property(e => e.DynamicFieldId).HasColumnName("DynamicFieldID");
            entity.Property(e => e.GroupField).HasMaxLength(50);
            entity.Property(e => e.IsFilterToolbar).HasDefaultValue(false);
            entity.Property(e => e.IsGrouped).HasDefaultValue(false);
            entity.Property(e => e.ReportCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Width).HasColumnName("width");
            entity.Property(e => e.ZoomLevel).HasMaxLength(50);
        });

        modelBuilder.Entity<NetForm>(entity =>
        {
            entity.ToTable("NET_Form");

            entity.Property(e => e.IsReceiveRealTime).HasDefaultValue(false);
            entity.Property(e => e.IsSendRealTime).HasDefaultValue(false);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetFormVersion>(entity =>
        {
            entity.ToTable("NET_Form_Version");

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_18_17").HasFillFactor(90);

            entity.HasIndex(e => e.HinFormId, "IX_MissingIndex_207_206").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_35_34").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_42_41").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_55_54").HasFillFactor(90);

            entity.Property(e => e.Apicontent).HasColumnName("APIContent");
            entity.Property(e => e.ExportMergeField).HasDefaultValue(false);
            entity.Property(e => e.HinFormCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PositionButton).HasDefaultValue(2);
            entity.Property(e => e.StoreCheckUrl).HasColumnName("StoreCheckURL");
        });

        modelBuilder.Entity<NetFormVersionField>(entity =>
        {
            entity.ToTable("NET_Form_VersionField");

            entity.HasIndex(e => new { e.HinFormVersionId, e.IsActive }, "IX_MissingIndex_209_208").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormVersionId, e.IsActive }, "IX_MissingIndex_86_85").HasFillFactor(90);

            entity.Property(e => e.HinFormCode)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetMenu>(entity =>
        {
            entity.ToTable("NET_Menu");

            entity.Property(e => e.Cssformat).HasColumnName("CSSFormat");
            entity.Property(e => e.CssiconFormat).HasColumnName("CSSIconFormat");
            entity.Property(e => e.IsCount)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TextColor).HasColumnName("textColor");
            entity.Property(e => e.TypeCheck)
                .HasDefaultValue(1)
                .HasColumnName("typeCheck");
        });

        modelBuilder.Entity<NetMenuRole>(entity =>
        {
            entity.ToTable("NET_MenuRole");

            entity.HasIndex(e => e.TenantId, "IX_NET_MenuRole_TenantId");
        });

        modelBuilder.Entity<NetReport>(entity =>
        {
            entity.ToTable("NET_Report");

            entity.Property(e => e.AllowedApi).HasColumnName("AllowedAPI");
            entity.Property(e => e.Cache).HasDefaultValue(false);
            entity.Property(e => e.DisableSearch).HasDefaultValue(false);
            entity.Property(e => e.FormId).HasColumnName("FormID");
            entity.Property(e => e.FunctionCode).HasMaxLength(100);
            entity.Property(e => e.IsAutoCollapse).HasDefaultValue(false);
            entity.Property(e => e.IsBackViewer).HasDefaultValue(false);
            entity.Property(e => e.IsBtnHandle).HasDefaultValue(true);
            entity.Property(e => e.IsCreateEditor).HasDefaultValue(true);
            entity.Property(e => e.IsDeleteEditor).HasDefaultValue(false);
            entity.Property(e => e.IsEditEditor).HasDefaultValue(true);
            entity.Property(e => e.IsExportExcel).HasDefaultValue(true);
            entity.Property(e => e.IsExportWord).HasDefaultValue(false);
            entity.Property(e => e.IsFreepane).HasDefaultValue(true);
            entity.Property(e => e.IsRecieveRealTime).HasDefaultValue(false);
            entity.Property(e => e.IsSearchbar).HasDefaultValue(true);
            entity.Property(e => e.MasterDetailReportCode).IsUnicode(false);
            entity.Property(e => e.Pagination).HasDefaultValue(false);
            entity.Property(e => e.SelectionType)
                .HasMaxLength(10)
                .HasDefaultValue("multiple");
            entity.Property(e => e.ShowHeaderFilter).HasDefaultValue(false);
            entity.Property(e => e.ShowIconFilter).HasDefaultValue(false);
            entity.Property(e => e.ShowPage).HasDefaultValue(true);
            entity.Property(e => e.ShowToolbar).HasDefaultValue(true);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SqlContentM).HasDefaultValue("");
            entity.Property(e => e.SqlTypeM).HasDefaultValue(false);
            entity.Property(e => e.StoreCheckUrl).HasColumnName("StoreCheckURL");
            entity.Property(e => e.StoreDrdisplay).HasColumnName("StoreDRDisplay");
        });

        modelBuilder.Entity<NetService>(entity =>
        {
            entity.ToTable("NET_Service");

            entity.Property(e => e.Cache).HasDefaultValue(true);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NetTabPanel>(entity =>
        {
            entity.ToTable("NET_TabPanel");

            entity.Property(e => e.AfterEffectIcon).HasMaxLength(100);
            entity.Property(e => e.AfterEffectIconColor).HasMaxLength(100);
            entity.Property(e => e.BeforeEffectIcon).HasMaxLength(100);
            entity.Property(e => e.BeforeEffectIconColor).HasMaxLength(100);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StoreCheckTabDetail).HasMaxLength(100);
            entity.Property(e => e.StoreCheckUrl).HasColumnName("StoreCheckURL");
            entity.Property(e => e.StorePermissionByRecord).HasMaxLength(100);
        });

        modelBuilder.Entity<NetTabPanelDetail>(entity =>
        {
            entity.ToTable("NET_TabPanel_Detail");

            entity.Property(e => e.HinTabPanelCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HinTabPanelId).HasColumnName("HinTabPanelID");
            entity.Property(e => e.IsLoop).HasDefaultValue(false);
        });

        modelBuilder.Entity<NetValidation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DRValida__3214EC07C8D7F405");

            entity.ToTable("NET_Validation");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(1)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(0)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Pattern).HasMaxLength(50);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Store).HasMaxLength(100);
        });

        modelBuilder.Entity<NetWidget>(entity =>
        {
            entity.ToTable("NET_Widget");
        });

        modelBuilder.Entity<NetWidgetDefaultConfig>(entity =>
        {
            entity.ToTable("NET_WidgetDefaultConfig");
        });

        modelBuilder.Entity<NetWidgetGroup>(entity =>
        {
            entity.ToTable("NET_WidgetGroup");
        });

        modelBuilder.Entity<NetWidgetItem>(entity =>
        {
            entity.ToTable("NET_WidgetItem");

            entity.Property(e => e.DataSourceId)
                .HasDefaultValue(0)
                .HasColumnName("DataSourceID");
        });

        modelBuilder.Entity<NetWidgetMap>(entity =>
        {
            entity.ToTable("NET_WidgetMap");
        });

        modelBuilder.Entity<NetWidgetValueConfig>(entity =>
        {
            entity.ToTable("NET_WidgetValueConfig");

            entity.HasIndex(e => new { e.WidgetItemId, e.IsDelete }, "IX_MissingIndex_4_3").HasFillFactor(90);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
