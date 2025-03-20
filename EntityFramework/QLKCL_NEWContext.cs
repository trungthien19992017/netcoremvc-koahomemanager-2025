using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KOAHome.EntityFramework;

public partial class QLKCL_NEWContext : DbContext
{
    public QLKCL_NEWContext(DbContextOptions<QLKCL_NEWContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<AttachmentSyntax> AttachmentSyntaxes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }

    public virtual DbSet<GetAttachment> GetAttachments { get; set; }

    public virtual DbSet<HsBooking> HsBookings { get; set; }

    public virtual DbSet<HsBookingService> HsBookingServices { get; set; }

    public virtual DbSet<HsChiPhi> HsChiPhis { get; set; }

    public virtual DbSet<HsCustomer> HsCustomers { get; set; }

    public virtual DbSet<HsDichVuTheoLich> HsDichVuTheoLiches { get; set; }

    public virtual DbSet<HsHomestay> HsHomestays { get; set; }

    public virtual DbSet<HsOwner> HsOwners { get; set; }

    public virtual DbSet<HsPayment> HsPayments { get; set; }

    public virtual DbSet<HsReview> HsReviews { get; set; }

    public virtual DbSet<HsRoom> HsRooms { get; set; }

    public virtual DbSet<HsService> HsServices { get; set; }

    public virtual DbSet<HsServiceHistory> HsServiceHistories { get; set; }

    public virtual DbSet<HsServicePriceByRoom> HsServicePriceByRooms { get; set; }

    public virtual DbSet<HsThongTinDatView> HsThongTinDatViews { get; set; }

    public virtual DbSet<UnitIdByUser> UnitIdByUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachment");

            entity.HasIndex(e => new { e.ObjectTypeId, e.ObjectId, e.IsDeleted }, "IX_Attachment_ObjectTypeId_ObjectId_IsDeleted");
        });

        modelBuilder.Entity<AttachmentSyntax>(entity =>
        {
            entity.ToTable("AttachmentSyntax");

            entity.Property(e => e.IsChangeSyntaxName).HasDefaultValue(false);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
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

        modelBuilder.Entity<CategoryDetail>(entity =>
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

        modelBuilder.Entity<GetAttachment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GetAttachment");

            entity.Property(e => e.ContentType).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.ConvertDiskDirectory).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.ConvertFileName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.DiskDirectory).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.DiskFileName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.FileName).UseCollation("Vietnamese_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<HsBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__HS_Booki__73951ACDEB4A6008");

            entity.ToTable("HS_Booking");

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
                .ToView("HS_DichVuTheoLich");

            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ListDichVu).HasMaxLength(4000);
        });

        modelBuilder.Entity<HsHomestay>(entity =>
        {
            entity.HasKey(e => e.HomestayId).HasName("PK__HS_Homes__EDCB5CDA19FC469C");

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
                .HasConstraintName("FK__HS_Homest__Owner__74CE504D");
        });

        modelBuilder.Entity<HsOwner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__HS_Owner__81938598BB8436F3");

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
            entity.HasKey(e => e.PaymentId).HasName("PK__HS_Payme__9B556A5823FBE87F");

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

        modelBuilder.Entity<HsReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__HS_Revie__74BC79AED17EBE47");

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
                .HasConstraintName("FK__HS_Review__RoomI__45F365D3");
        });

        modelBuilder.Entity<HsRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__HS_Room__328639197901063C");

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

            entity.HasOne(d => d.Homestay).WithMany(p => p.HsRooms)
                .HasForeignKey(d => d.HomestayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HS_Room__Homesta__08D548FA");
        });

        modelBuilder.Entity<HsService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__HS_Servi__C51BB0EA62A40F9B");

            entity.ToTable("HS_Service");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FromHour).HasMaxLength(50);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
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
            entity.HasKey(e => e.Id).HasName("PK__HS_Servi__3214EC07690B79DB");

            entity.ToTable("HS_Service_History");

            entity.Property(e => e.CreationTime).HasColumnType("datetime");
            entity.Property(e => e.DeletionTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("isDeleted");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("IsRead");
            entity.Property(e => e.LastModificationTime).HasColumnType("datetime");
            entity.Property(e => e.TableName).HasMaxLength(100);
        });

        modelBuilder.Entity<HsServicePriceByRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HS_Servi__3214EC07B0ADDA09");

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

        modelBuilder.Entity<UnitIdByUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UnitIdByUser");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
