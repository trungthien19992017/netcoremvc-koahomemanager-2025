using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KOAHome.Migrations.TttConfig
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "net_action",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    ispopupconfirm = table.Column<bool>(type: "boolean", nullable: true),
                    confirmtitle = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    confirmtext = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    confirmbuttontext = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_action", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_actionlist",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    objectid = table.Column<int>(type: "integer", nullable: true),
                    objectcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    actionlisttypeid = table.Column<int>(type: "integer", nullable: true),
                    actionlisttypecode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("pk_net_actionlist", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_actionlistdetail",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<string>(type: "text", nullable: true),
                    actionlistid = table.Column<int>(type: "integer", nullable: true),
                    actionlistcode = table.Column<string>(type: "text", nullable: true),
                    actionid = table.Column<int>(type: "integer", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: true),
                    icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    displayname = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<int>(type: "integer", nullable: true),
                    istop = table.Column<bool>(type: "boolean", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    ischecksamepopup = table.Column<bool>(type: "boolean", nullable: true),
                    checksamepopuptext = table.Column<string>(type: "text", nullable: true),
                    checksamepopupbutton = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    urlimportfile = table.Column<string>(type: "text", nullable: true),
                    filetypeaccept = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    confirmbuttontext = table.Column<string>(type: "text", nullable: true),
                    confirmtitle = table.Column<string>(type: "text", nullable: true),
                    confirmtext = table.Column<string>(type: "text", nullable: true),
                    ispopupconfirm = table.Column<bool>(type: "boolean", nullable: true),
                    ischoosedata = table.Column<bool>(type: "boolean", nullable: true),
                    isgroup = table.Column<bool>(type: "boolean", nullable: true),
                    idgroup = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    errorcol = table.Column<string>(type: "text", nullable: true),
                    filetemplate = table.Column<string>(type: "text", nullable: true),
                    typenodediagram = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "text", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleteuserid = table.Column<int>(type: "integer", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    issendrealtime = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    codesendrealtime = table.Column<string>(type: "text", nullable: true),
                    servicefilename = table.Column<int>(type: "integer", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: true),
                    cssbutton = table.Column<string>(type: "text", nullable: true),
                    iszoompopup = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isnet_actionhowerror = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_actionlistdetail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_actiontype",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_actiontype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_attachment",
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
                    Order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_attachment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_attachmentsyntax",
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
                    table.PrimaryKey("pk_net_attachmentsyntax", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_dashboard",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    dashboardcode = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    options = table.Column<string>(type: "text", nullable: true),
                    storedefault = table.Column<string>(type: "text", nullable: true),
                    showcalendarfilter = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    textcalendarcolor = table.Column<string>(type: "text", nullable: true),
                    autoreload = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    datasourceid = table.Column<int>(type: "integer", nullable: true),
                    codereceiverealtime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hin_dashboards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_dashboardpage",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pagecode = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    dashboardid = table.Column<int>(type: "integer", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_dashboardpage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_datasource",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    sqltype = table.Column<string>(type: "text", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_datasource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_datasourcedetail",
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
                    configid = table.Column<long>(type: "bigint", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: true),
                    isdefault = table.Column<bool>(type: "boolean", nullable: true),
                    host = table.Column<string>(type: "text", nullable: true),
                    dbname = table.Column<string>(type: "text", nullable: true),
                    user = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    timeout = table.Column<int>(type: "integer", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_datasourcedetail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_display",
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
                    table.PrimaryKey("pk_net_display", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_dynamicfield",
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
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_dynamicfield", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_filter",
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
                    dynamicfieldid = table.Column<long>(type: "bigint", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    combolevel = table.Column<int>(type: "integer", nullable: true),
                    parentcomboid = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    datatype = table.Column<bool>(type: "boolean", nullable: true),
                    lookupid = table.Column<long>(type: "bigint", nullable: true),
                    seviceid = table.Column<long>(type: "bigint", nullable: true),
                    disable = table.Column<bool>(type: "boolean", nullable: true),
                    required = table.Column<bool>(type: "boolean", nullable: true),
                    width = table.Column<string>(type: "text", nullable: true),
                    format = table.Column<string>(type: "text", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: true),
                    orderid = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isvalue = table.Column<bool>(type: "boolean", nullable: true),
                    groupid = table.Column<int>(type: "integer", nullable: true),
                    colspan = table.Column<int>(type: "integer", nullable: true),
                    colcount = table.Column<int>(type: "integer", nullable: true),
                    isgrouped = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    groupfield = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isloadmultipleway = table.Column<bool>(type: "boolean", nullable: true),
                    columns = table.Column<string>(type: "text", nullable: true),
                    zoomlevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    datedisplayformat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    multicontrolid = table.Column<string>(type: "text", nullable: true),
                    reportcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    isfiltertoolbar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_filter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_form",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    latestversion = table.Column<int>(type: "integer", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cssoptionheader = table.Column<string>(type: "text", nullable: true),
                    codereceiverealtime = table.Column<string>(type: "text", nullable: true),
                    issendrealtime = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isreceiverealtime = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    codesendrealtime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_form", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_form_version",
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
                    hinformid = table.Column<long>(type: "bigint", nullable: true),
                    hinformbookvalueid = table.Column<long>(type: "bigint", nullable: true),
                    version = table.Column<int>(type: "integer", nullable: true),
                    options = table.Column<string>(type: "text", nullable: true),
                    tablename = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<int>(type: "integer", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    objectcode = table.Column<string>(type: "text", nullable: true),
                    storegetdata = table.Column<string>(type: "text", nullable: true),
                    storesetdata = table.Column<string>(type: "text", nullable: true),
                    isback = table.Column<bool>(type: "boolean", nullable: true),
                    storedefaultdata = table.Column<string>(type: "text", nullable: true),
                    isview = table.Column<bool>(type: "boolean", nullable: true),
                    storelabelaction = table.Column<string>(type: "text", nullable: true),
                    storesetreadonly = table.Column<string>(type: "text", nullable: true),
                    storecheckurl = table.Column<string>(type: "text", nullable: true),
                    hinformcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    positionbutton = table.Column<int>(type: "integer", nullable: true),
                    apicontent = table.Column<string>(type: "text", nullable: true),
                    exportmergefield = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    saveeditortype = table.Column<int>(type: "integer", nullable: true),
                    conditionofaction = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_form_version", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_form_versionfield",
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
                    hinformversionid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    parentid = table.Column<long>(type: "bigint", nullable: true),
                    parentcode = table.Column<string>(type: "text", nullable: true),
                    fieldtypeid = table.Column<long>(type: "bigint", nullable: true),
                    options = table.Column<string>(type: "text", nullable: true),
                    validates = table.Column<string>(type: "text", nullable: true),
                    datasources = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    tabindex = table.Column<int>(type: "integer", nullable: true),
                    hinformcode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_form_versionfield", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_formfieldtype",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isrowtemplate = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_formfieldtype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_mainmenu",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    parent = table.Column<int>(type: "integer", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: true),
                    requiredpermissionname = table.Column<string>(type: "text", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    devicetype = table.Column<int>(type: "integer", nullable: true),
                    ismobile = table.Column<bool>(type: "boolean", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isminiitem = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    imageurl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_mainmenu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_menu",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    mobilelink = table.Column<string>(type: "text", nullable: true),
                    parent = table.Column<int>(type: "integer", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: true),
                    requiredpermissionname = table.Column<string>(type: "text", nullable: true),
                    menuid = table.Column<int>(type: "integer", nullable: true),
                    sqlstring = table.Column<string>(type: "text", nullable: true),
                    iscount = table.Column<bool>(type: "boolean", nullable: true),
                    countnum = table.Column<int>(type: "integer", nullable: true),
                    organizationid = table.Column<int>(type: "integer", nullable: true),
                    israwsql = table.Column<bool>(type: "boolean", nullable: true),
                    sqlcountstore = table.Column<string>(type: "text", nullable: true),
                    countoutofdate = table.Column<int>(type: "integer", nullable: true),
                    cssformat = table.Column<string>(type: "text", nullable: true),
                    cssiconformat = table.Column<string>(type: "text", nullable: true),
                    counttype = table.Column<int>(type: "integer", nullable: true),
                    parentorgid = table.Column<int>(type: "integer", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true),
                    iframe = table.Column<string>(type: "text", nullable: true),
                    imageurl = table.Column<string>(type: "text", nullable: true),
                    textcolor = table.Column<string>(type: "text", nullable: true),
                    typecheck = table.Column<int>(type: "integer", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_menu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_menurole",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tenantid = table.Column<int>(type: "integer", nullable: true),
                    roleid = table.Column<int>(type: "integer", nullable: true),
                    labelid = table.Column<int>(type: "integer", nullable: true),
                    menuid = table.Column<int>(type: "integer", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    rolemappergroupid = table.Column<int>(type: "integer", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_menurole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_report",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    sqltype = table.Column<bool>(type: "boolean", nullable: true),
                    sqlcontent = table.Column<string>(type: "text", nullable: true),
                    grouplevel = table.Column<long>(type: "bigint", nullable: true),
                    excel = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<long>(type: "bigint", nullable: true),
                    layoutpfilter = table.Column<string>(type: "text", nullable: true),
                    displaytype = table.Column<int>(type: "integer", nullable: true),
                    isdynamiccolumn = table.Column<bool>(type: "boolean", nullable: true),
                    formid = table.Column<long>(type: "bigint", nullable: true),
                    typegetcolumn = table.Column<int>(type: "integer", nullable: true),
                    isexportword = table.Column<bool>(type: "boolean", nullable: true),
                    word = table.Column<string>(type: "text", nullable: true),
                    sqleditcontent = table.Column<string>(type: "text", nullable: true),
                    sqldefaultcontent = table.Column<string>(type: "text", nullable: true),
                    sqlstoredlabelaction = table.Column<string>(type: "text", nullable: true),
                    disablesearch = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    colspan = table.Column<int>(type: "integer", nullable: true),
                    colcount = table.Column<int>(type: "integer", nullable: true),
                    sqledittemplatecontent = table.Column<string>(type: "text", nullable: true),
                    sqlexportdata = table.Column<string>(type: "text", nullable: true),
                    sqlexportfield = table.Column<string>(type: "text", nullable: true),
                    allowedpagesizes = table.Column<string>(type: "text", nullable: true),
                    disablehandlecollumn = table.Column<bool>(type: "boolean", nullable: true),
                    storedrdisplay = table.Column<string>(type: "text", nullable: true),
                    isautocollapse = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    sqltypem = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    sqlcontentm = table.Column<string>(type: "text", nullable: true),
                    isediteditor = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    iscreateeditor = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isbtnhandle = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isexportexcel = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    selectiontype = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    storecheckurl = table.Column<string>(type: "text", nullable: true),
                    isbackviewer = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    issearchbar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    isfreepane = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    defaultparam = table.Column<string>(type: "text", nullable: true),
                    positionbutton = table.Column<int>(type: "integer", nullable: true),
                    reporttype = table.Column<int>(type: "integer", nullable: true),
                    storedrag = table.Column<string>(type: "text", nullable: true),
                    editingmode = table.Column<string>(type: "text", nullable: true),
                    showheaderfilter = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    functioncode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    funtionid = table.Column<int>(type: "integer", nullable: true),
                    showpage = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    showtoolbar = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    chartviewdisplay = table.Column<int>(type: "integer", nullable: true),
                    reportcoderecieverealtime = table.Column<string>(type: "text", nullable: true),
                    showiconfilter = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isrecieverealtime = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isdeleteeditor = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    servicehiddenfilter = table.Column<int>(type: "integer", nullable: true),
                    templateids = table.Column<string>(type: "text", nullable: true),
                    cache = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    allowedapi = table.Column<string>(type: "text", nullable: true),
                    pagination = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    enablemasterdetail = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    masterdetailreportcode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_report", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_service",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<long>(type: "bigint", nullable: true),
                    sqltype = table.Column<bool>(type: "boolean", nullable: true),
                    sqlcontent = table.Column<string>(type: "text", nullable: true),
                    colvalue = table.Column<string>(type: "text", nullable: true),
                    coldisplay = table.Column<string>(type: "text", nullable: true),
                    colparent = table.Column<string>(type: "text", nullable: true),
                    cache = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    storeddefaultparam = table.Column<string>(type: "text", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_service", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_tabpanel",
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
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    storegetdata = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<int>(type: "integer", nullable: true),
                    filetemplate = table.Column<string>(type: "text", nullable: true),
                    storeexportfile = table.Column<string>(type: "text", nullable: true),
                    selectedindex = table.Column<int>(type: "integer", nullable: true),
                    iseffecticon = table.Column<bool>(type: "boolean", nullable: true),
                    storechecktabdetail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    beforeeffecticon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    beforeeffecticoncolor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    aftereffecticon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    aftereffecticoncolor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ispermission = table.Column<bool>(type: "boolean", nullable: true),
                    ispermissionbyrecord = table.Column<bool>(type: "boolean", nullable: true),
                    storepermissionbyrecord = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    storecheckurl = table.Column<string>(type: "text", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    storegetfieldexportform = table.Column<string>(type: "text", nullable: true),
                    storegetfieldexportdatagrid = table.Column<string>(type: "text", nullable: true),
                    isexportexcel = table.Column<bool>(type: "boolean", nullable: true),
                    isexportwordtemplate = table.Column<bool>(type: "boolean", nullable: true),
                    storecountnotify = table.Column<string>(type: "text", nullable: true),
                    storetabpermission = table.Column<string>(type: "text", nullable: true),
                    servicegetfilename = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_tabpanel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_tabpanel_detail",
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
                    hintabpanelid = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    template = table.Column<string>(type: "text", nullable: true),
                    options = table.Column<string>(type: "text", nullable: true),
                    orderid = table.Column<long>(type: "bigint", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    isloop = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    storeloop = table.Column<string>(type: "text", nullable: true),
                    hintabpanelcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tabicon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_tabpanel_detail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_validation",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    min = table.Column<int>(type: "integer", nullable: true),
                    max = table.Column<int>(type: "integer", nullable: true),
                    pattern = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    store = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    message = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    isactive = table.Column<int>(type: "integer", nullable: true),
                    isdeleted = table.Column<int>(type: "integer", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<int>(type: "integer", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<int>(type: "integer", nullable: true),
                    deleteruserid = table.Column<int>(type: "integer", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    siteid = table.Column<int>(type: "integer", nullable: true),
                    sitecode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    datasourceid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__drvalida__3214ec07c8d7f405", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widget",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    widgetcode = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    displaytypecode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widget", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widgetdefaultconfig",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    defaultvalue = table.Column<string>(type: "text", nullable: true),
                    widgetlayoutid = table.Column<int>(type: "integer", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    indexnumber = table.Column<int>(type: "integer", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widgetdefaultconfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widgetgroup",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupname = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widgetgroup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widgetitem",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    widgetlayoutid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    groupwidgetid = table.Column<int>(type: "integer", nullable: true),
                    imgreview = table.Column<string>(type: "text", nullable: true),
                    datasourceid = table.Column<int>(type: "integer", nullable: true),
                    templateids = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widgetitem", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widgetmap",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    widgetitemid = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    pageid = table.Column<int>(type: "integer", nullable: true),
                    positionx = table.Column<int>(type: "integer", nullable: true),
                    positiony = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true),
                    creationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creatoruserid = table.Column<long>(type: "bigint", nullable: true),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lastmodifieruserid = table.Column<long>(type: "bigint", nullable: true),
                    deleteruserid = table.Column<long>(type: "bigint", nullable: true),
                    deletiontime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    indexnumber = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widgetmap", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "net_widgetvalueconfig",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    widgetitemid = table.Column<int>(type: "integer", nullable: true),
                    isdelete = table.Column<bool>(type: "boolean", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    index = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_net_widgetvalueconfig", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tempqueries",
                schema: "dbo",
                columns: table => new
                {
                    rownum = table.Column<long>(type: "bigint", nullable: true),
                    col1 = table.Column<string>(type: "character varying(376)", maxLength: 376, nullable: true),
                    col2 = table.Column<string>(type: "text", nullable: true),
                    col3 = table.Column<string>(type: "character varying(259)", maxLength: 259, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tempqueriescopy",
                schema: "dbo",
                columns: table => new
                {
                    rownum = table.Column<long>(type: "bigint", nullable: true),
                    col1 = table.Column<string>(type: "character varying(376)", maxLength: 376, nullable: true),
                    col2 = table.Column<string>(type: "text", nullable: true),
                    col3 = table.Column<string>(type: "character varying(259)", maxLength: 259, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "widgetlayout_test",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    widgetid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    userid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    dashboardid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk__widgetla__3214ec07b34c3338", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_ix_net_attachment_objecttypeid_objectid_isdeleted",
                schema: "dbo",
                table: "net_attachment",
                columns: new[] { "objecttypeid", "objectid", "isdeleted" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_61_60",
                schema: "dbo",
                table: "net_display",
                column: "reportid");

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_64_63",
                schema: "dbo",
                table: "net_display",
                columns: new[] { "isdeleted", "reportid" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_12_11",
                schema: "dbo",
                table: "net_filter",
                columns: new[] { "isdeleted", "reportid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_536_535",
                schema: "dbo",
                table: "net_filter",
                columns: new[] { "isdeleted", "reportid", "dynamicfieldid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_568_567",
                schema: "dbo",
                table: "net_filter",
                columns: new[] { "reportid", "dynamicfieldid" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_6_5",
                schema: "dbo",
                table: "net_filter",
                columns: new[] { "isdeleted", "reportid" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_8_7",
                schema: "dbo",
                table: "net_filter",
                columns: new[] { "isdeleted", "reportid" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_18_17",
                schema: "dbo",
                table: "net_form_version",
                columns: new[] { "isdeleted", "hinformid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_207_206",
                schema: "dbo",
                table: "net_form_version",
                column: "hinformid");

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_35_34",
                schema: "dbo",
                table: "net_form_version",
                columns: new[] { "isdeleted", "hinformid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_42_41",
                schema: "dbo",
                table: "net_form_version",
                columns: new[] { "isdeleted", "hinformid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_55_54",
                schema: "dbo",
                table: "net_form_version",
                columns: new[] { "isdeleted", "hinformid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_209_208",
                schema: "dbo",
                table: "net_form_versionfield",
                columns: new[] { "hinformversionid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_86_85",
                schema: "dbo",
                table: "net_form_versionfield",
                columns: new[] { "isdeleted", "hinformversionid", "isactive" });

            migrationBuilder.CreateIndex(
                name: "idx_ix_net_menurole_tenantid",
                schema: "dbo",
                table: "net_menurole",
                column: "tenantid");

            migrationBuilder.CreateIndex(
                name: "idx_ix_missingindex_4_3",
                schema: "dbo",
                table: "net_widgetvalueconfig",
                columns: new[] { "widgetitemid", "isdelete" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "net_action",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_actionlist",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_actionlistdetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_actiontype",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_attachment",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_attachmentsyntax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_dashboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_dashboardpage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_datasource",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_datasourcedetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_display",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_dynamicfield",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_filter",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_form",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_form_version",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_form_versionfield",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_formfieldtype",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_mainmenu",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_menu",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_menurole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_report",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_service",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_tabpanel",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_tabpanel_detail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_validation",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widget",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widgetdefaultconfig",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widgetgroup",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widgetitem",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widgetmap",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "net_widgetvalueconfig",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tempqueries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tempqueriescopy",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "widgetlayout_test",
                schema: "dbo");
        }
    }
}
