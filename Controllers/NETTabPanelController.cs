using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Diagnostics;

namespace KOAHome.Controllers
{
  public class NETTabPanelController : Controller
  {
    private readonly ILogger<NETTabPanelController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IActionService _action;
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;
    private readonly INetServiceService _netService;
    private readonly IConnectionService _con;
    private readonly INetTabPanelService _tab;

    public NETTabPanelController(QLKCL_NEWContext db, ILogger<NETTabPanelController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con, INetTabPanelService tab)
    {
      _db = db;
      _logger = logger;
      _re = re;
      _att = att;
      _report = report;
      _form = form;
      _action = action;
      _widget = widget;
      _datasrc = datasrc;
      _netService = netService;
      _con = con;
      _tab = tab;
    }

    // GET: NETTabPanelController
    [HttpGet]
    public async Task<ActionResult> Viewer(string TabCode, int? TabIndex)
    {
      try
      {
        // neu không trả về menu code thì chuyển sang link lỗi
        if (TabCode == null)
        {
          ViewData["ErrorMessage"] = "Không tồn tại mã tab";
          return View();
        }
        ViewData["TabCode"] = TabCode;
        ViewData["TabIndex"] = TabIndex;

        // Lấy dynamic query parameters
        var parameters = Request.Query;
        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());
        // đưa popup param qua view để chuyển cho các partial nếu có
        ViewData["objParameters"] = objParameters;

        // lấy thông tin tab panel
        var tabpanel = await _tab.NET_TabPanel_Get(TabCode);
        // tra ve page loi neu khong tim thay tab panel
        if (tabpanel == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy thông tin tab";
          return View();
        }
        // chuyển thông tin tab panel lên view
        ViewData["tabpanel"] = tabpanel;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (tabpanel.ContainsKey("datasourceid"))
        {
          if (tabpanel["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(tabpanel["datasourceid"]));
          }
        }

        // lay thong tin tab panel, va danh sach chi tiết cua tab panel de xu ly
        var tabpaneldetail = await _tab.NET_TabPanel_Detail_Get(TabCode, null);
        // tra ve page loi neu khong tim thay menu
        if (tabpaneldetail == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy chi tiết tab";
          return View();
        }
        // chuyen cau hinh menu len giao dien de xu ly
        ViewData["tabpaneldetail"] = tabpaneldetail;

        ViewData["sucess"] = "Thành công";

        return View();
      }
      catch (SqlException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }
    }

    // GET: NETTabPanelController/Details/5
    public ActionResult Details(int id)
    {
      return View();
    }

    // GET: NETTabPanelController/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: NETTabPanelController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: NETTabPanelController/Edit/5
    public ActionResult Edit(int id)
    {
      return View();
    }

    // POST: NETTabPanelController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }

    // GET: NETTabPanelController/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: NETTabPanelController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
      try
      {
        return RedirectToAction(nameof(Index));
      }
      catch
      {
        return View();
      }
    }
  }
}
