using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KOAHome.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:dblink", ",,")
                .Annotation("Npgsql:PostgresExtension:postgres_fdw", ",,");

            migrationBuilder.CreateTable(
                name: "__efmigrationshistory",
                schema: "dbo",
                columns: table => new
                {
                    migrationid = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    productversion = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk___efmigrationshistory", x => x.migrationid);
                });

            migrationBuilder.CreateTable(
                name: "abproles",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    concurrencystamp = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    displayname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    isdefault = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    isstatic = table.Column<bool>(type: "boolean", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    normalizedname = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    pageredirect = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    loai = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    siteid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    defaultmenuid = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    unitid = table.Column<int>(type: "integer", nullable: true),
                    parentid = table.Column<int>(type: "integer", nullable: true),
                    roletype = table.Column<int>(type: "integer", nullable: true),
                    labelid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_abproles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abpuserroles",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    roleid = table.Column<int>(type: "integer", nullable: true),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: true),
                    rolelevel = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_abpuserroles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "abpusers",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accessfailedcount = table.Column<int>(type: "integer", nullable: true),
                    authenticationsource = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    concurrencystamp = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    emailaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    emailconfirmationcode = table.Column<string>(type: "character varying(328)", maxLength: 328, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    isemailconfirmed = table.Column<bool>(type: "boolean", nullable: true),
                    islockoutenabled = table.Column<bool>(type: "boolean", nullable: true),
                    isphonenumberconfirmed = table.Column<bool>(type: "boolean", nullable: true),
                    istwofactorenabled = table.Column<bool>(type: "boolean", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    lockoutenddateutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    normalizedemailaddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    passwordresetcode = table.Column<string>(type: "character varying(328)", maxLength: 328, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    profilepictureid = table.Column<Guid>(type: "uuid", nullable: true),
                    securitystamp = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    shouldchangepasswordonnextlogin = table.Column<bool>(type: "boolean", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    signintoken = table.Column<string>(type: "text", nullable: true),
                    signintokenexpiretimeutc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    googleauthenticatorkey = table.Column<string>(type: "text", nullable: true),
                    position = table.Column<string>(type: "text", nullable: true),
                    apikey = table.Column<string>(type: "text", nullable: true),
                    unitid = table.Column<int>(type: "integer", nullable: true),
                    useridkcl = table.Column<Guid>(type: "uuid", nullable: true),
                    issynckcl = table.Column<int>(type: "integer", nullable: true),
                    useruid = table.Column<string>(type: "text", nullable: true),
                    phongbanid = table.Column<int>(type: "integer", nullable: true),
                    useridcchc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    userid_qlvb = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    signatureimg = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_abpusers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attachment",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    filename = table.Column<string>(type: "text", nullable: true),
                    diskfilename = table.Column<string>(type: "text", nullable: true),
                    filesize = table.Column<int>(type: "integer", nullable: true),
                    contenttype = table.Column<string>(type: "text", nullable: true),
                    authorid = table.Column<long>(type: "bigint", nullable: true),
                    diskdirectory = table.Column<string>(type: "text", nullable: true),
                    objectid = table.Column<long>(type: "bigint", nullable: true),
                    objecttypeid = table.Column<long>(type: "bigint", nullable: true),
                    iscurrent = table.Column<bool>(type: "boolean", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: true),
                    convertfilename = table.Column<string>(type: "text", nullable: true),
                    convertdiskdirectory = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attachmentsyntax",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    syntaxpath = table.Column<string>(type: "text", nullable: true),
                    isdefault = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    ischangesyntaxname = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    syntaxname = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachmentsyntax", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    typeid = table.Column<int>(type: "integer", nullable: true),
                    typecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categorydetail",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryid = table.Column<int>(type: "integer", nullable: true),
                    categorycode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorydetail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                schema: "dbo",
                columns: table => new
                {
                    cityid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    citycode = table.Column<string>(type: "text", nullable: true),
                    cityname = table.Column<string>(type: "text", nullable: true),
                    indexid = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cities", x => x.cityid);
                });

            migrationBuilder.CreateTable(
                name: "ctkm_t10d",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    diemtu = table.Column<int>(type: "integer", nullable: true),
                    diemden = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true),
                    havevoucher = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__ctkm_t10d__a4ae64b8304c97e3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ctkm_t10d_de",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    time = table.Column<int>(type: "integer", nullable: true),
                    kmid = table.Column<int>(type: "integer", nullable: true),
                    kmcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__ctkm_t10d_de__a4ae64b8304c97e3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "districts",
                schema: "dbo",
                columns: table => new
                {
                    districtid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cityid = table.Column<long>(type: "bigint", nullable: true),
                    districtcode = table.Column<string>(type: "text", nullable: true),
                    districtname = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    danso = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_districts", x => x.districtid);
                });

            migrationBuilder.CreateTable(
                name: "drdisplay",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reportid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    format = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    width = table.Column<string>(type: "text", nullable: true),
                    colnum = table.Column<int>(type: "integer", nullable: true),
                    grouplevel = table.Column<int>(type: "integer", nullable: true),
                    isdisplay = table.Column<bool>(type: "boolean", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: true),
                    textalign = table.Column<string>(type: "text", nullable: true),
                    textissum = table.Column<string>(type: "text", nullable: true),
                    issum = table.Column<bool>(type: "boolean", nullable: true),
                    iscount = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    ismax = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    ismin = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isfreepane = table.Column<bool>(type: "boolean", nullable: true),
                    isparent = table.Column<bool>(type: "boolean", nullable: true),
                    parentcode = table.Column<string>(type: "text", nullable: true),
                    groupsort = table.Column<string>(type: "text", nullable: true),
                    isreadonly = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isexport = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    serviceid = table.Column<int>(type: "integer", nullable: true),
                    validationrule = table.Column<string>(type: "text", nullable: true),
                    sortbycolumn = table.Column<string>(type: "text", nullable: true),
                    visible = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    editcelltemplate = table.Column<string>(type: "text", nullable: true),
                    editcolumns = table.Column<string>(type: "text", nullable: true),
                    columnsetdata = table.Column<string>(type: "text", nullable: true),
                    showingroupfooter = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    configpopup = table.Column<string>(type: "text", nullable: true),
                    configheader = table.Column<string>(type: "text", nullable: true),
                    isexpand = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    area = table.Column<string>(type: "text", nullable: true),
                    pivotfield = table.Column<string>(type: "text", nullable: true),
                    summarydisplaymode = table.Column<string>(type: "text", nullable: true),
                    isavg = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    pivotorders = table.Column<string>(type: "text", nullable: true),
                    reportcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    issort = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    freepaneposition = table.Column<string>(type: "text", nullable: true),
                    cssheader = table.Column<string>(type: "text", nullable: true),
                    formulasyntax = table.Column<string>(type: "text", nullable: true),
                    allowmergecells = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    customsummary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drdisplay", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genders",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__genders__3214ec07bb181d81", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hs_chiphi",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    expensedatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    totalamount = table.Column<double>(type: "double precision", nullable: true),
                    ischeck = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_chiph__3214ec07c7f42e9c", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hs_customer",
                schema: "dbo",
                columns: table => new
                {
                    customerid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    dateofbirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    cccd = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    mxh = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_custo__a4ae64b8304c97e3", x => x.customerid);
                });

            migrationBuilder.CreateTable(
                name: "hs_owner",
                schema: "dbo",
                columns: table => new
                {
                    ownerid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phonenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_owner__81938598b243a403", x => x.ownerid);
                });

            migrationBuilder.CreateTable(
                name: "hs_service",
                schema: "dbo",
                columns: table => new
                {
                    serviceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ishourservice = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    formula = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    ispricebyroom = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true),
                    fromhour = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tohour = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    serviceimage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    intimerange = table.Column<int>(type: "integer", nullable: true),
                    isaddon = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    applydate = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_servi__c51bb0ea62a40f9b", x => x.serviceid);
                });

            migrationBuilder.CreateTable(
                name: "hs_service_history",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tablename = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    objectid = table.Column<int>(type: "integer", nullable: true),
                    information = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true),
                    isread = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_servi__3214ec07a38cb74e", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hs_servicepricebyroom",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roomid = table.Column<int>(type: "integer", nullable: true),
                    serviceid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    serviceprice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_servi__3214ec0782b239f1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nationalities",
                schema: "dbo",
                columns: table => new
                {
                    nationalityid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    nationalitycode = table.Column<string>(type: "text", nullable: true),
                    nationalityname = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nationalities", x => x.nationalityid);
                });

            migrationBuilder.CreateTable(
                name: "nations",
                schema: "dbo",
                columns: table => new
                {
                    nationid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    nationcode = table.Column<string>(type: "text", nullable: true),
                    nationname = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nations", x => x.nationid);
                });

            migrationBuilder.CreateTable(
                name: "radioyesno",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "refreshtoken",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<long>(type: "bigint", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true),
                    jwtid = table.Column<string>(type: "text", nullable: true),
                    isused = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isrevoked = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    issuedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expiredat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refreshtoken", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wards",
                schema: "dbo",
                columns: table => new
                {
                    wardid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    districtid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    wardcode = table.Column<string>(type: "text", nullable: true),
                    wardname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wards", x => x.wardid);
                });

            migrationBuilder.CreateTable(
                name: "hs_homestay",
                schema: "dbo",
                columns: table => new
                {
                    homestayid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ownerid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_homes__edcb5cda4c5dec80", x => x.homestayid);
                    table.ForeignKey(
                        name: "fk__hs_homest__owner__6aefe058",
                        column: x => x.ownerid,
                        principalSchema: "dbo",
                        principalTable: "hs_owner",
                        principalColumn: "ownerid");
                });

            migrationBuilder.CreateTable(
                name: "hs_room",
                schema: "dbo",
                columns: table => new
                {
                    roomid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    homestayid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    badgeclass = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    roomimage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_room__328639198358319a", x => x.roomid);
                    table.ForeignKey(
                        name: "fk__hs_room__homesta__6cd828ca",
                        column: x => x.homestayid,
                        principalSchema: "dbo",
                        principalTable: "hs_homestay",
                        principalColumn: "homestayid");
                });

            migrationBuilder.CreateTable(
                name: "hs_booking",
                schema: "dbo",
                columns: table => new
                {
                    bookingid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    roomid = table.Column<int>(type: "integer", nullable: true),
                    checkindate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    checkoutdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    bookingdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    unitprice = table.Column<double>(type: "double precision", nullable: true),
                    totalamount = table.Column<double>(type: "double precision", nullable: true),
                    deposit = table.Column<double>(type: "double precision", nullable: true),
                    totaltime = table.Column<double>(type: "double precision", nullable: true),
                    otherphonenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    discountpercent = table.Column<double>(type: "double precision", nullable: true),
                    otherdiscountamount = table.Column<double>(type: "double precision", nullable: true),
                    reasondiscount = table.Column<string>(type: "text", nullable: true),
                    ispay = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    reasoncancel = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_booki__73951acdeb4a6008", x => x.bookingid);
                    table.ForeignKey(
                        name: "fk__hs_bookin__custo__2c578814",
                        column: x => x.customerid,
                        principalSchema: "dbo",
                        principalTable: "hs_customer",
                        principalColumn: "customerid");
                    table.ForeignKey(
                        name: "fk__hs_bookin__roomi__2d4bac4d",
                        column: x => x.roomid,
                        principalSchema: "dbo",
                        principalTable: "hs_room",
                        principalColumn: "roomid");
                });

            migrationBuilder.CreateTable(
                name: "hs_review",
                schema: "dbo",
                columns: table => new
                {
                    reviewid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<int>(type: "integer", nullable: true),
                    roomid = table.Column<int>(type: "integer", nullable: true),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    reviewdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_revie__74bc79aea8c7fe8b", x => x.reviewid);
                    table.ForeignKey(
                        name: "fk__hs_review__custo__1844d718",
                        column: x => x.customerid,
                        principalSchema: "dbo",
                        principalTable: "hs_customer",
                        principalColumn: "customerid");
                    table.ForeignKey(
                        name: "fk__hs_review__roomi__5535a963",
                        column: x => x.roomid,
                        principalSchema: "dbo",
                        principalTable: "hs_room",
                        principalColumn: "roomid");
                });

            migrationBuilder.CreateTable(
                name: "hs_bookingservice",
                schema: "dbo",
                columns: table => new
                {
                    bookingserviceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookingid = table.Column<int>(type: "integer", nullable: true),
                    serviceid = table.Column<int>(type: "integer", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    totalprice = table.Column<double>(type: "double precision", nullable: true),
                    additionfromdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    additiontodate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_booki__43f55cd171b565df", x => x.bookingserviceid);
                    table.ForeignKey(
                        name: "fk__hs_bookin__booki__36d51687",
                        column: x => x.bookingid,
                        principalSchema: "dbo",
                        principalTable: "hs_booking",
                        principalColumn: "bookingid");
                    table.ForeignKey(
                        name: "fk__hs_bookin__servi__48e80e73",
                        column: x => x.serviceid,
                        principalSchema: "dbo",
                        principalTable: "hs_service",
                        principalColumn: "serviceid");
                });

            migrationBuilder.CreateTable(
                name: "hs_payment",
                schema: "dbo",
                columns: table => new
                {
                    paymentid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bookingid = table.Column<int>(type: "integer", nullable: true),
                    amount = table.Column<double>(type: "double precision", nullable: true),
                    paymentdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    paymentmethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    paymentinformation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__hs_payme__9b556a58fd157a61", x => x.paymentid);
                    table.ForeignKey(
                        name: "fk__hs_paymen__booki__4246c933",
                        column: x => x.bookingid,
                        principalSchema: "dbo",
                        principalTable: "hs_booking",
                        principalColumn: "bookingid");
                });

            migrationBuilder.CreateIndex(
                name: "idx_ix_attachment_objecttypeid_objectid_isdeleted",
                schema: "dbo",
                table: "attachment",
                columns: new[] { "objecttypeid", "objectid", "isdeleted" });

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_checkindate",
                schema: "dbo",
                table: "hs_booking",
                column: "checkindate");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_checkoutdate",
                schema: "dbo",
                table: "hs_booking",
                column: "checkoutdate");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_customerid",
                schema: "dbo",
                table: "hs_booking",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_isactive",
                schema: "dbo",
                table: "hs_booking",
                column: "isactive");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_isdeleted",
                schema: "dbo",
                table: "hs_booking",
                column: "isdeleted");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_ispay",
                schema: "dbo",
                table: "hs_booking",
                column: "ispay");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_roomid",
                schema: "dbo",
                table: "hs_booking",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_bookingservice_bookingid",
                schema: "dbo",
                table: "hs_bookingservice",
                column: "bookingid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_bookingservice_serviceid",
                schema: "dbo",
                table: "hs_bookingservice",
                column: "serviceid");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_hoten",
                schema: "dbo",
                table: "hs_customer",
                column: "lastname");

            migrationBuilder.CreateIndex(
                name: "idx_nonclusteredindex_phonenumber",
                schema: "dbo",
                table: "hs_customer",
                column: "phonenumber");

            migrationBuilder.CreateIndex(
                name: "IX_hs_homestay_ownerid",
                schema: "dbo",
                table: "hs_homestay",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_payment_bookingid",
                schema: "dbo",
                table: "hs_payment",
                column: "bookingid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_review_customerid",
                schema: "dbo",
                table: "hs_review",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_review_roomid",
                schema: "dbo",
                table: "hs_review",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_hs_room_homestayid",
                schema: "dbo",
                table: "hs_room",
                column: "homestayid");

            migrationBuilder.CreateIndex(
                name: "idx_ix_hs_service_history_tablename_creationtime",
                schema: "dbo",
                table: "hs_service_history",
                columns: new[] { "tablename", "creationtime" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_hs_service_history_tablename_isdeleted",
                schema: "dbo",
                table: "hs_service_history",
                columns: new[] { "tablename", "isdeleted" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_wards_districtid",
                schema: "dbo",
                table: "wards",
                column: "districtid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__efmigrationshistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "abproles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "abpuserroles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "abpusers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "attachment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "attachmentsyntax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "category",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "categorydetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "cities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ctkm_t10d",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ctkm_t10d_de",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "districts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "drdisplay",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "genders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_bookingservice",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_chiphi",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_payment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_review",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_service_history",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_servicepricebyroom",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "nationalities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "nations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "radioyesno",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "refreshtoken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "wards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_service",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_booking",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_customer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_room",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_homestay",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "hs_owner",
                schema: "dbo");
        }
    }
}
