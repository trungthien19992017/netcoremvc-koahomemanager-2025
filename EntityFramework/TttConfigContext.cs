using System;
using System.Collections.Generic;
using KOAHome.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KOAHome.EntityFramework;

public partial class TttConfigContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    int,
    IdentityUserClaim<int>,
    IdentityUserRole<int>,
    IdentityUserLogin<int>,
    IdentityRoleClaim<int>,
    IdentityUserToken<int>>
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

    public virtual DbSet<NetActionType> NetActionTypes { get; set; }

    public virtual DbSet<NetAttachment> NetAttachments { get; set; }

    public virtual DbSet<NetAttachmentSyntax> NetAttachmentSyntaxes { get; set; }

    public virtual DbSet<NetDashboard> NetDashboards { get; set; }

    public virtual DbSet<NetDashboardPage> NetDashboardPages { get; set; }

    public virtual DbSet<NetDataSource> NetDataSources { get; set; }

    public virtual DbSet<NetDataSourceDetail> NetDataSourceDetails { get; set; }

    public virtual DbSet<NetDisplay> NetDisplays { get; set; }

    public virtual DbSet<NetDynamicField> NetDynamicFields { get; set; }

    public virtual DbSet<NetFilter> NetFilters { get; set; }

    public virtual DbSet<NetForm> NetForms { get; set; }

    public virtual DbSet<NetFormFieldType> NetFormFieldTypes { get; set; }

    public virtual DbSet<NetFormVersion> NetFormVersions { get; set; }

    public virtual DbSet<NetFormVersionField> NetFormVersionFields { get; set; }

    public virtual DbSet<NetMainMenu> NetMainMenus { get; set; }

    public virtual DbSet<NetMenu> NetMenus { get; set; }

    public virtual DbSet<NetMenuRole> NetMenuRoles { get; set; }

    public virtual DbSet<NetReport> NetReports { get; set; }

    public virtual DbSet<NetService> NetServices { get; set; }

    public virtual DbSet<NetStepper> NetSteppers { get; set; }

    public virtual DbSet<NetStepperDetail> NetStepperDetails { get; set; }

    public virtual DbSet<NetTabPanel> NetTabPanels { get; set; }

    public virtual DbSet<NetTabPanelDetail> NetTabPanelDetails { get; set; }

    public virtual DbSet<NetTenant> NetTenants { get; set; }

    public virtual DbSet<NetUnit> NetUnits { get; set; }

    public virtual DbSet<NetValidation> NetValidations { get; set; }

    public virtual DbSet<NetWidget> NetWidgets { get; set; }

    public virtual DbSet<NetWidgetDefaultConfig> NetWidgetDefaultConfigs { get; set; }

    public virtual DbSet<NetWidgetGroup> NetWidgetGroups { get; set; }

    public virtual DbSet<NetWidgetItem> NetWidgetItems { get; set; }

    public virtual DbSet<NetWidgetMap> NetWidgetMaps { get; set; }

    public virtual DbSet<NetWidgetValueConfig> NetWidgetValueConfigs { get; set; }

    public virtual DbSet<WidgetLayoutTest> WidgetLayoutTests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ConfigConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetAction>(entity =>
        {
            entity.ToTable("NET_Action");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmButtonText)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmText)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmTitle)
                .HasMaxLength(10)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Icon).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetActionList>(entity =>
        {
            entity.ToTable("NET_ActionList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionListTypeCode)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Code)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(1000)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ObjectCode)
                .HasMaxLength(200)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetActionListDetail>(entity =>
        {
            entity.ToTable("NET_ActionListDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionListCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CheckSamePopupButton)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CheckSamePopupText).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CodeSendRealTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmButtonText).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmText).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfirmTitle).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.CssButton).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DataSourceId).HasColumnName("DataSourceID");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.DisplayName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ErrorCol).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FileTemplate).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FileTypeAccept)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Icon)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Idgroup)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("IDGroup");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsNetActionhowError).HasColumnName("IsNET_ActionhowError");
            entity.Property(e => e.IsSendRealTime).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.RoleId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Type).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TypeNodeDiagram)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UrlImportFile).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Value).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetActionType>(entity =>
        {
            entity.ToTable("NET_ActionType");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_net_attachment");

            entity.HasIndex(e => new { e.ObjectTypeId, e.ObjectId, e.IsDeleted }, "IX_NET_Attachment_ObjectTypeId_ObjectId_IsDeleted");

            entity.Property(e => e.ContentType).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConvertDiskDirectory).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConvertFileName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetAttachmentSyntax>(entity =>
        {
            entity.ToTable("NET_AttachmentSyntax");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsChangeSyntaxName).HasDefaultValue(false);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SyntaxName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SyntaxPath).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HIN_Dashboards");

            entity.ToTable("NET_Dashboard");

            entity.Property(e => e.CodeReceiveRealTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DashboardCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Options).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreDefault).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TextCalendarColor).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDashboardPage>(entity =>
        {
            entity.ToTable("NET_DashboardPage");

            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PageCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDataSource>(entity =>
        {
            entity.ToTable("NET_DataSource");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlType).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDataSourceDetail>(entity =>
        {
            entity.ToTable("NET_DataSourceDetail");

            entity.Property(e => e.Dbname)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("DBName");
            entity.Property(e => e.Host).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.User).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDisplay>(entity =>
        {
            entity.ToTable("NET_Display");

            entity.HasIndex(e => e.ReportId, "IX_MissingIndex_61_60").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.ReportId }, "IX_MissingIndex_64_63").HasFillFactor(90);

            entity.Property(e => e.Area).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ColumnSetData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfigHeader).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ConfigPopup).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CssHeader).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CustomSummary)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EditCellTemplate).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EditColumns).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Format).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FormulaSyntax)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FreePanePosition).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.GroupSort).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsAvg).HasDefaultValue(false);
            entity.Property(e => e.IsCount).HasDefaultValue(false);
            entity.Property(e => e.IsExpand).HasDefaultValue(false);
            entity.Property(e => e.IsMax).HasDefaultValue(false);
            entity.Property(e => e.IsMin).HasDefaultValue(false);
            entity.Property(e => e.IsSort).HasDefaultValue(false);
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ParentCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PivotField).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PivotOrders).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ReportCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShowInGroupFooter).HasDefaultValue(false);
            entity.Property(e => e.SortByColumn).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SummaryDisplayMode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TextAlign).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TextIsSum).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Type).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ValidationRule).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Visible).HasDefaultValue(true);
            entity.Property(e => e.Width).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetDynamicField>(entity =>
        {
            entity.ToTable("NET_DynamicField");

            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Type).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Value).UseCollation("SQL_Latin1_General_CP1_CI_AS");
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
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Width).HasColumnName("width");
            entity.Property(e => e.ZoomLevel).HasMaxLength(50);
        });

        modelBuilder.Entity<NetForm>(entity =>
        {
            entity.ToTable("NET_Form");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CodeReceiveRealTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CodeSendRealTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CssOptionHeader).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsReceiveRealTime).HasDefaultValue(false);
            entity.Property(e => e.IsSendRealTime).HasDefaultValue(false);
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetFormFieldType>(entity =>
        {
            entity.ToTable("NET_FormFieldType");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Icon).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsRowTemplate).HasDefaultValue(false);
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetFormVersion>(entity =>
        {
            entity.ToTable("NET_Form_Version");

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_18_17").HasFillFactor(90);

            entity.HasIndex(e => e.HinFormId, "IX_MissingIndex_207_206").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_35_34").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_42_41").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormId, e.IsActive }, "IX_MissingIndex_55_54").HasFillFactor(90);

            entity.Property(e => e.Apicontent)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("APIContent");
            entity.Property(e => e.ConditionOfAction).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ExportMergeField).HasDefaultValue(false);
            entity.Property(e => e.HinFormCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PositionButton).HasDefaultValue(2);
            entity.Property(e => e.StoreCheckUrl)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("StoreCheckURL");
        });

        modelBuilder.Entity<NetFormVersionField>(entity =>
        {
            entity.ToTable("NET_Form_VersionField");

            entity.HasIndex(e => new { e.HinFormVersionId, e.IsActive }, "IX_MissingIndex_209_208").HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.HinFormVersionId, e.IsActive }, "IX_MissingIndex_86_85").HasFillFactor(90);

            entity.Property(e => e.Datasources).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.HinFormCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Options).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ParentCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Validates).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetMainMenu>(entity =>
        {
            entity.ToTable("NET_MainMenu");

            entity.Property(e => e.Code)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Icon).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImageUrl).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsMiniItem).HasDefaultValue(false);
            entity.Property(e => e.Link).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RequiredPermissionName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetMenu>(entity =>
        {
            entity.ToTable("NET_Menu");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Cssformat)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CSSFormat");
            entity.Property(e => e.CssiconFormat)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("CSSIconFormat");
            entity.Property(e => e.Description).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Icon).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Iframe).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImageUrl).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsCount)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),(0)))");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Link).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MobileLink).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RequiredPermissionName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlCountStore).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlString).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TextColor)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("textColor");
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1_CI_AS");
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

            entity.Property(e => e.AllowedApi)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("AllowedAPI");
            entity.Property(e => e.AllowedPageSizes).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Cache).HasDefaultValue(false);
            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DefaultParam).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DisableSearch).HasDefaultValue(false);
            entity.Property(e => e.EditingMode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Excel).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FormId).HasColumnName("FormID");
            entity.Property(e => e.FunctionCode)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
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
            entity.Property(e => e.LayoutpFilter).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MasterDetailReportCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Pagination).HasDefaultValue(false);
            entity.Property(e => e.ReportCodeRecieveRealTime).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SelectionType)
                .HasMaxLength(10)
                .HasDefaultValue("multiple")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShowHeaderFilter).HasDefaultValue(false);
            entity.Property(e => e.ShowIconFilter).HasDefaultValue(false);
            entity.Property(e => e.ShowPage).HasDefaultValue(true);
            entity.Property(e => e.ShowToolbar).HasDefaultValue(true);
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlContent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlContentM)
                .HasDefaultValue("")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlDefaultContent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlEditContent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlEditTemplateContent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlExportData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlExportField).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlStoredLabelAction).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlTypeM).HasDefaultValue(false);
            entity.Property(e => e.StoreCheckUrl)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("StoreCheckURL");
            entity.Property(e => e.StoreDrag).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreDrdisplay)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("StoreDRDisplay");
            entity.Property(e => e.TemplateIds).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Word).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetService>(entity =>
        {
            entity.ToTable("NET_Service");

            entity.Property(e => e.Cache).HasDefaultValue(true);
            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ColDisplay).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ColParent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ColValue).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SqlContent).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoredDefaultParam).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetStepper>(entity =>
        {
            entity.ToTable("NET_Stepper");

            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.IsViewOnly).HasDefaultValue(false);
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreDefaultData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreGetData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreLoadDynamicData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreSetData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetStepperDetail>(entity =>
        {
            entity.ToTable("NET_Stepper_Detail");

            entity.Property(e => e.HinWorkflowCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LabelActionCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetTabPanel>(entity =>
        {
            entity.ToTable("NET_TabPanel");

            entity.Property(e => e.AfterEffectIcon)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AfterEffectIconColor)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.BeforeEffectIcon)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.BeforeEffectIconColor)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Code).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FileTemplate).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreCheckTabDetail)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreCheckUrl)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("StoreCheckURL");
            entity.Property(e => e.StoreCountNotify).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreExportFile).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreGetData).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreGetFieldExportDatagrid).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreGetFieldExportForm).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StorePermissionByRecord)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreTabPermission).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreCheckUrl).HasColumnName("StoreCheckURL");
            entity.Property(e => e.StorePermissionByRecord).HasMaxLength(100);
        modelBuilder.Entity<NetTabPanelDetail>(entity =>

            entity.ToTable("NET_TabPanel_Detail");

            entity.Property(e => e.HinTabPanelCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.HinTabPanelId).HasColumnName("HinTabPanelID");
            entity.Property(e => e.IsLoop).HasDefaultValue(false);
            entity.Property(e => e.Options).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.StoreLoop).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TabIcon)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TabIconColor)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Template).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Title).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<NetTenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NET_Tena__3214EC075157A3F7");

            entity.ToTable("NET_Tenant");

            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.StartNumberProd).HasColumnName("StartNumberPROD");
            entity.Property(e => e.TenantIcoUrl).HasMaxLength(500);
            entity.Property(e => e.TenantLogoTextUrl).HasMaxLength(500);
            entity.Property(e => e.TenantLogoUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<NetUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NET_Unit__3214EC074D0A0272");

            entity.ToTable("NET_Unit");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsParent).HasColumnName("isParent");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.LeadUserId).HasColumnName("LeadUserID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.UnitName).HasMaxLength(1024);
            entity.Property(e => e.UnitType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HinTabPanelId).HasColumnName("HinTabPanelID");
            entity.Property(e => e.IsLoop).HasDefaultValue(false);
        });

            entity.HasKey(e => e.Id).HasName("PK__DRValida__3214EC07C8D7F405");
        {
            entity.ToTable("NET_Validation");

            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(1)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(0)
                .HasColumnName("isDeleted");
            entity.Property(e => e.Key)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Message)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Pattern)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.SiteCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Store)
            entity.Property(e => e.Pattern).HasMaxLength(50);
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
                .IsUnicode(false);
            entity.Property(e => e.Store).HasMaxLength(100);
        });

            entity.ToTable("NET_Widget");

            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DisplayTypeCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WidgetCode).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        {
            entity.ToTable("NET_Widget");
        modelBuilder.Entity<NetWidgetDefaultConfig>(entity =>

            entity.ToTable("NET_WidgetDefaultConfig");

            entity.Property(e => e.DefaultValue).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Key).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        {
            entity.ToTable("NET_WidgetDefaultConfig");
        modelBuilder.Entity<NetWidgetGroup>(entity =>

            entity.ToTable("NET_WidgetGroup");

            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.GroupName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        {
            entity.ToTable("NET_WidgetGroup");
        modelBuilder.Entity<NetWidgetItem>(entity =>

            entity.ToTable("NET_WidgetItem");
                .HasNoKey()
            entity.Property(e => e.DataSourceId)
                .HasDefaultValue(0)
                .HasColumnName("DataSourceID");
            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ImgReview).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TemplateIds).UseCollation("SQL_Latin1_General_CP1_CI_AS");
                .HasDefaultValue(0)
                .HasColumnName("DataSourceID");
        modelBuilder.Entity<NetWidgetMap>(entity =>

            entity.ToTable("NET_WidgetMap");

            entity.Property(e => e.Descriptions).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        {
            entity.ToTable("NET_WidgetMap");
        modelBuilder.Entity<NetWidgetValueConfig>(entity =>

            entity.ToTable("NET_WidgetValueConfig");

            entity.HasIndex(e => new { e.WidgetItemId, e.IsDelete }, "IX_MissingIndex_4_3").HasFillFactor(90);
                .HasColumnName("widgetid");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<WidgetLayoutTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WidgetLa__3214EC07C50AE4CF");

            entity.ToTable("WidgetLayout_Test");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DashboardId)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.WidgetId)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        // Cuối cùng phải có dòng này:
        base.OnModelCreating(modelBuilder); // ⚠️ Quan trọng
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
