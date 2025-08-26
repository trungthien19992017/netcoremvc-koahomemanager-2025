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

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentSyntax> AttachmentSyntaxes { get; set; }

    public virtual DbSet<BookingFilterMaterializedView> BookingFilterMaterializedViews { get; set; }

    public virtual DbSet<BookingListMaterializedView> BookingListMaterializedViews { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CtkmT10d> CtkmT10ds { get; set; }

    public virtual DbSet<CtkmT10dDe> CtkmT10dDes { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Drdisplay> Drdisplays { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GetAttachment> GetAttachments { get; set; }

    public virtual DbSet<HinCategory> HinCategories { get; set; }

    public virtual DbSet<HinCategoryDetail> HinCategoryDetails { get; set; }

    public virtual DbSet<HinCity> HinCities { get; set; }

    public virtual DbSet<HinDistrict> HinDistricts { get; set; }

    public virtual DbSet<HinWard> HinWards { get; set; }

    public virtual DbSet<HsBooking> HsBookings { get; set; }

    public virtual DbSet<HsBookingService> HsBookingServices { get; set; }

    public virtual DbSet<HsCategory> HsCategories { get; set; }

    public virtual DbSet<HsCategoryDetail> HsCategoryDetails { get; set; }

    public virtual DbSet<HsChiPhi> HsChiPhis { get; set; }

    public virtual DbSet<HsCustomer> HsCustomers { get; set; }

    public virtual DbSet<HsDichVuTheoLich> HsDichVuTheoLiches { get; set; }

    public virtual DbSet<HsHomestay> HsHomestays { get; set; }

    public virtual DbSet<HsMediaPlaylist> HsMediaPlaylists { get; set; }

    public virtual DbSet<HsOwner> HsOwners { get; set; }

    public virtual DbSet<HsPayment> HsPayments { get; set; }

    public virtual DbSet<HsPromotion> HsPromotions { get; set; }

    public virtual DbSet<HsPromotionCondition> HsPromotionConditions { get; set; }

    public virtual DbSet<HsPromotionReward> HsPromotionRewards { get; set; }

    public virtual DbSet<HsReview> HsReviews { get; set; }

    public virtual DbSet<HsRoom> HsRooms { get; set; }

    public virtual DbSet<HsService> HsServices { get; set; }

    public virtual DbSet<HsServiceHistory> HsServiceHistories { get; set; }

    public virtual DbSet<HsServicePriceByRoom> HsServicePriceByRooms { get; set; }

    public virtual DbSet<HsThongTinDatView> HsThongTinDatViews { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Nationality> Nationalities { get; set; }

    public virtual DbSet<NetAttachmentSyntaxView> NetAttachmentSyntaxViews { get; set; }

    public virtual DbSet<RadioYesNo> RadioYesNos { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<UnitIdByUser> UnitIdByUsers { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_attachment");

            entity.HasIndex(e => new { e.ObjectTypeId, e.ObjectId, e.IsDeleted }, "IX_Attachment_ObjectTypeId_ObjectId_IsDeleted");
        });

        modelBuilder.Entity<AttachmentSyntax>(entity =>
        {
            entity.ToTable("AttachmentSyntax");

            entity.Property(e => e.IsChangeSyntaxName).HasDefaultValue(false);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
        });

        modelBuilder.Entity<BookingFilterMaterializedView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BookingFilter_MaterializedView");

            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
        });

        modelBuilder.Entity<BookingListMaterializedView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BookingList_MaterializedView");

            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .HasColumnName("CCCD");
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Gender).HasMaxLength(3);
            entity.Property(e => e.GenderColorClass).HasMaxLength(11);
            entity.Property(e => e.GenderIcon).HasMaxLength(13);
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsPayClass)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(51);
            entity.Property(e => e.Mxh)
                .HasMaxLength(200)
                .HasColumnName("MXH");
            entity.Property(e => e.Name).HasMaxLength(399);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.PromoLink)
                .HasMaxLength(158)
                .IsUnicode(false);
            entity.Property(e => e.RoomBadgeClass).HasMaxLength(50);
            entity.Property(e => e.RoomName).HasMaxLength(203);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
            entity.Property(e => e.TypeCode).HasMaxLength(100);
        });

        modelBuilder.Entity<Categorydetail>(entity =>
        {
            entity.ToTable("CategoryDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryCode).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
        });

        modelBuilder.Entity<CtkmT10d>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CTKM_T10D__A4AE64B8304C97E3");

            entity.ToTable("CTKM_T10D");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.HaveVoucher)
                .HasDefaultValue(false)
                .HasColumnName("haveVoucher");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CtkmT10dDe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CTKM_T10D_DE__A4AE64B8304C97E3");

            entity.ToTable("CTKM_T10D_DE");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.Kmcode)
                .HasMaxLength(50)
                .HasColumnName("KMCode");
            entity.Property(e => e.Kmid).HasColumnName("KMId");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Drdisplay>(entity =>
        {
            entity.ToTable("DRDisplay");

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

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genders__3214EC07BB181D81");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<GetAttachment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GetAttachment");

            entity.Property(e => e.DiskDirectory).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.DiskFileName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.FileName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<HinCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HIN_Category");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
            entity.Property(e => e.TypeCode).HasMaxLength(100);
        });

        modelBuilder.Entity<HinCategoryDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HIN_CategoryDetail");

            entity.Property(e => e.CategoryCode).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
        });

        modelBuilder.Entity<HinCity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HIN_Cities");

            entity.Property(e => e.CityId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<HinDistrict>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HIN_Districts");

            entity.Property(e => e.DistrictId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<HinWard>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HIN_Wards");

            entity.Property(e => e.WardId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<HsBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HS_Booki__73951ACDEB4A6008");

            entity.ToTable("HS_Booking");

            entity.HasIndex(e => e.CheckInDate, "NonClusteredIndex-CheckInDate");

            entity.HasIndex(e => e.CheckOutDate, "NonClusteredIndex-CheckOutDate");

            entity.HasIndex(e => e.CustomerId, "NonClusteredIndex-CustomerID");

            entity.HasIndex(e => e.IsActive, "NonClusteredIndex-IsActive");

            entity.HasIndex(e => e.IsDeleted, "NonClusteredIndex-IsDeleted");

            entity.HasIndex(e => e.IsPay, "NonClusteredIndex-IsPay");

            entity.HasIndex(e => e.RoomId, "NonClusteredIndex-RoomID");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.CheckInDate).HasColumnType("datetime");
            entity.Property(e => e.CheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Deposit)
                .HasDefaultValue(0.0)
                .HasComment("tien coc");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsPay).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.OtherPhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Customer).WithMany(p => p.HsBookings)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Bookin__Custo__2C578814");

            entity.HasOne(d => d.Room).WithMany(p => p.HsBookings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Bookin__RoomI__2D4BAC4D");
        });

        modelBuilder.Entity<HsBookingService>(entity =>
        {
            entity.HasKey(e => e.BookingServiceId).HasName("PK__HS_Booki__43F55CD171B565DF");

            entity.ToTable("HS_BookingService");

            entity.Property(e => e.BookingServiceId).HasColumnName("BookingServiceID");
            entity.Property(e => e.AdditionFromDate).HasColumnType("datetime");
            entity.Property(e => e.AdditionToDate).HasColumnType("datetime");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Booking).WithMany(p => p.HsBookingServices)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Bookin__Booki__36D51687");

            entity.HasOne(d => d.Service).WithMany(p => p.HsBookingServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Bookin__Servi__48E80E73");
        });

        modelBuilder.Entity<HsCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Categ__3214EC27360E6190");

            entity.ToTable("HS_Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
            entity.Property(e => e.TypeCode).HasMaxLength(100);
        });

        modelBuilder.Entity<HsCategoryDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Categ__3214EC278283D4FB");

            entity.ToTable("HS_CategoryDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CategoryCode).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.Data).HasMaxLength(500);
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(1000);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SiteCode).HasMaxLength(50);
        });

        modelBuilder.Entity<HsChiPhi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_ChiPh__3214EC07C7F42E9C");

            entity.ToTable("HS_ChiPhi");

            entity.Property(e => e.Content).HasMaxLength(200);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.ExpenseDatetime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsCheck).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<HsCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__HS_Custo__A4AE64B8304C97E3");

            entity.ToTable("HS_Customer");

            entity.HasIndex(e => e.LastName, "NonClusteredIndex-HoTen");

            entity.HasIndex(e => e.PhoneNumber, "NonClusteredIndex-PhoneNumber");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .HasColumnName("CCCD");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mxh)
                .HasMaxLength(200)
                .HasColumnName("MXH");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<HsDichVuTheoLich>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("hs_dichvutheolich", "dbo");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ListDichVu).HasMaxLength(4000);
        });

        modelBuilder.Entity<HsHomestay>(entity =>
        {
            entity.HasKey(e => e.HomestayId).HasName("PK__HS_Homes__EDCB5CDA4C5DEC80");

            entity.ToTable("HS_Homestay");

            entity.Property(e => e.HomestayId).HasColumnName("HomestayID");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

            entity.HasOne(d => d.Owner).WithMany(p => p.HsHomestays)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Homest__Owner__025D5595");
        });

        modelBuilder.Entity<HsMediaPlaylist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Media__3214EC079EBC85F9");

            entity.ToTable("HS_Media_Playlist");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsChoosen).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.MediaPath).HasMaxLength(1000);
            entity.Property(e => e.MediaTitle).HasMaxLength(255);
            entity.Property(e => e.MediaType).HasMaxLength(50);
            entity.Property(e => e.ThumbnailPath).HasMaxLength(1000);
        });

        modelBuilder.Entity<HsOwner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__HS_Owner__81938598B243A403");

            entity.ToTable("HS_Owner");

            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<HsPayment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__HS_Payme__9B556A58FD157A61");

            entity.ToTable("HS_Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentInformation).HasMaxLength(100);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithMany(p => p.HsPayments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Paymen__Booki__4246C933");
        });

        modelBuilder.Entity<HsPromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Promo__3214EC07C919EF74");

            entity.ToTable("HS_Promotion");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsApplyOncePerUser).HasDefaultValue(false);
            entity.Property(e => e.IsAutomationApply).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PromotionTypeCode).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<HsPromotionCondition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Promo__3214EC0724EF2599");

            entity.ToTable("HS_PromotionCondition");

            entity.Property(e => e.ConditionOperator).HasMaxLength(10);
            entity.Property(e => e.ConditionSqltype)
                .HasMaxLength(100)
                .HasColumnName("ConditionSQLType");
            entity.Property(e => e.ConditionTypeCode).HasMaxLength(100);
            entity.Property(e => e.ConditionValue).HasMaxLength(255);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<HsPromotionReward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Promo__3214EC07F15F1E11");

            entity.ToTable("HS_PromotionReward");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.RewardTypeCode).HasMaxLength(50);
            entity.Property(e => e.RewardValue).HasMaxLength(100);
        });

        modelBuilder.Entity<HsReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__HS_Revie__74BC79AEA8C7FE8B");

            entity.ToTable("HS_Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.ReviewDate).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");

            entity.HasOne(d => d.Customer).WithMany(p => p.HsReviews)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Review__Custo__1844D718");

            entity.HasOne(d => d.Room).WithMany(p => p.HsReviews)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Review__RoomI__0539C240");
        });

        modelBuilder.Entity<HsRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__HS_Room__328639198358319A");

            entity.ToTable("HS_Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.BadgeClass).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.HomestayId).HasColumnName("HomestayID");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Number).HasMaxLength(100);
            entity.Property(e => e.RoomImage).HasMaxLength(100);

            entity.HasOne(d => d.Homestay).WithMany(p => p.HsRooms)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Room__Homesta__062DE679");
        });

        modelBuilder.Entity<HsService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__HS_Servi__C51BB0EA62A40F9B");

            entity.ToTable("HS_Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ApplyDate)
                .HasMaxLength(50)
                .HasComment("Áp dụng cho thứ mấy?");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FromHour).HasMaxLength(50);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsAddOn).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsHourService).HasDefaultValue(true);
            entity.Property(e => e.IsPriceByRoom).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ServiceImage).HasMaxLength(100);
            entity.Property(e => e.ToHour).HasMaxLength(50);
        });

        modelBuilder.Entity<HsServiceHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Servi__3214EC07A38CB74E");

            entity.ToTable("HS_Service_History");

            entity.HasIndex(e => new { e.TableName, e.CreationTime }, "IX_HS_Service_History_TableName_CreationTime");

            entity.HasIndex(e => new { e.TableName, e.IsDeleted }, "IX_HS_Service_History_TableName_IsDeleted");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<HsServicePriceByRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Servi__3214EC0782B239F1");

            entity.ToTable("HS_ServicePriceByRoom");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServicePrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<HsThongTinDatView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HS_ThongTinDat_View");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.CheckInInfo).HasMaxLength(4000);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
        });

        modelBuilder.Entity<NetAttachmentSyntaxView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("NET_AttachmentSyntax_View");

            entity.Property(e => e.Code).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.SyntaxName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.SyntaxPath).UseCollation("Vietnamese_CI_AS");
        });

        modelBuilder.Entity<RadioYesNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RadioYesNo");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken");

            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            entity.Property(e => e.IsUsed).HasDefaultValue(false);
            entity.Property(e => e.IssuedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UnitIdByUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UnitIdByUser");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasIndex(e => e.DistrictId, "IX_Wards_DistrictId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
