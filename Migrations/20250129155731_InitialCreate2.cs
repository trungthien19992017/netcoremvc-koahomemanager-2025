using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KOAHome.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HS_ChiPhi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseDatetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    IsCheck = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HS_ChiPhi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HS_Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MXH = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Custo__A4AE64B8304C97E3", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "HS_Owner",
                columns: table => new
                {
                    OwnerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Owner__81938598BB8436F3", x => x.OwnerID);
                });

            migrationBuilder.CreateTable(
                name: "HS_Service",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsHourService = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    Formula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    IsPriceByRoom = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true),
                    FromHour = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ToHour = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Servi__C51BB0EA62A40F9B", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "HS_Service_History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ObjectId = table.Column<int>(type: "int", nullable: true),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HS_Service_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HS_ServicePriceByRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HS_ServicePriceByRoom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HS_Homestay",
                columns: table => new
                {
                    HomestayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Homes__EDCB5CDA19FC469C", x => x.HomestayID);
                    table.ForeignKey(
                        name: "FK__HS_Homest__Owner__4B0D20AB",
                        column: x => x.OwnerID,
                        principalTable: "HS_Owner",
                        principalColumn: "OwnerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HS_Room",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomestayID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Room__328639197901063C", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK__HS_Room__Homesta__4C0144E4",
                        column: x => x.HomestayID,
                        principalTable: "HS_Homestay",
                        principalColumn: "HomestayID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HS_Booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    Deposit = table.Column<double>(type: "float", nullable: true, defaultValueSql: "((0))", comment: "tien coc"),
                    TotalTime = table.Column<double>(type: "float", nullable: true),
                    OtherPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DiscountPercent = table.Column<double>(type: "float", nullable: true),
                    OtherDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    ReasonDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPay = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ReasonCancel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Booki__73951ACDEB4A6008", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK__HS_Bookin__Custo__2C578814",
                        column: x => x.CustomerID,
                        principalTable: "HS_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__HS_Bookin__RoomI__2D4BAC4D",
                        column: x => x.RoomID,
                        principalTable: "HS_Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HS_Review",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Revie__74BC79AED17EBE47", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK__HS_Review__Custo__1844D718",
                        column: x => x.CustomerID,
                        principalTable: "HS_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__HS_Review__RoomI__45F365D3",
                        column: x => x.RoomID,
                        principalTable: "HS_Room",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HS_BookingService",
                columns: table => new
                {
                    BookingServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<double>(type: "float", nullable: true),
                    AdditionFromDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AdditionToDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Booki__43F55CD171B565DF", x => x.BookingServiceID);
                    table.ForeignKey(
                        name: "FK__HS_Bookin__Booki__36D51687",
                        column: x => x.BookingID,
                        principalTable: "HS_Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__HS_Bookin__Servi__48E80E73",
                        column: x => x.ServiceID,
                        principalTable: "HS_Service",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HS_Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentInformation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((1))"),
                    isDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    CreationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatorUserId = table.Column<int>(type: "int", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifierUserId = table.Column<int>(type: "int", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HS_Payme__9B556A5823FBE87F", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK__HS_Paymen__Booki__4246C933",
                        column: x => x.BookingID,
                        principalTable: "HS_Booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HS_Booking_CustomerID",
                table: "HS_Booking",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Booking_RoomID",
                table: "HS_Booking",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_BookingService_BookingID",
                table: "HS_BookingService",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_BookingService_ServiceID",
                table: "HS_BookingService",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Homestay_OwnerID",
                table: "HS_Homestay",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Payment_BookingID",
                table: "HS_Payment",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Review_CustomerID",
                table: "HS_Review",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Review_RoomID",
                table: "HS_Review",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_HS_Room_HomestayID",
                table: "HS_Room",
                column: "HomestayID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HS_BookingService");

            migrationBuilder.DropTable(
                name: "HS_ChiPhi");

            migrationBuilder.DropTable(
                name: "HS_Payment");

            migrationBuilder.DropTable(
                name: "HS_Review");

            migrationBuilder.DropTable(
                name: "HS_Service_History");

            migrationBuilder.DropTable(
                name: "HS_ServicePriceByRoom");

            migrationBuilder.DropTable(
                name: "HS_Service");

            migrationBuilder.DropTable(
                name: "HS_Booking");

            migrationBuilder.DropTable(
                name: "HS_Customer");

            migrationBuilder.DropTable(
                name: "HS_Room");

            migrationBuilder.DropTable(
                name: "HS_Homestay");

            migrationBuilder.DropTable(
                name: "HS_Owner");
        }
    }
}
