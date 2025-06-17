using AspnetCoreMvcFull.Models;
using KOAHome.EntityFramework;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Diagnostics;

namespace KOAHome.Controllers
{
  public class NETFormWizardController : Controller
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
    private readonly INetFormWizardService _stepper;

    public NETFormWizardController(QLKCL_NEWContext db, ILogger<NETTabPanelController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con, INetTabPanelService tab, INetFormWizardService stepper)
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
      _stepper = stepper;
    }

    // GET: NETFormWizardController
    public async Task<ActionResult> Viewer(string StepperCode)
    {
      try
      {
        // neu không trả về Stepper code thì chuyển sang link lỗi
        if (StepperCode == null)
        {
          ViewData["ErrorMessage"] = "Không tồn tại mã quy trình";
          return View();
        }
        ViewData["StepperCode"] = StepperCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;
        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());
        // đưa popup param qua view để chuyển cho các partial nếu có
        ViewData["objParameters"] = objParameters;

        // lấy thông tin stepper
        var stepper = await _stepper.NET_FormWizard_Get(StepperCode);
        // tra ve page loi neu khong tim thay tab panel
        if (stepper == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy thông tin quy trình";
          return View();
        }
        // chuyển thông tin tab panel lên view
        ViewData["stepper"] = stepper;

        string? connectionString = null;
        //neu datasourceId la null thi lay connectionString mac dinh
        if (stepper.ContainsKey("datasourceid"))
        {
          if (stepper["datasourceid"] != null)
          {
            //lay connectionstring tu report de goi store
            connectionString = await _datasrc.GetConnectionString(Convert.ToInt32(stepper["datasourceid"]));
          }
        }

        // lay thong tin tab panel, va danh sach chi tiết cua tab panel de xu ly
        var stepperdetail = await _stepper.NET_FormWizard_Detail_Get(StepperCode, null);
        // tra ve page loi neu khong tim thay menu
        if (stepperdetail == null)
        {
          ViewData["ErrorMessage"] = "Không tìm thấy chi tiết quy trình";
          return View();
        }
        // chuyen cau hinh menu len giao dien de xu ly
        ViewData["stepperdetail"] = stepperdetail;

        ViewData["sucess"] = "Thành công";

        return View();
      }
      catch (PostgresException ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form wizard.");
        // Optionally, return an error view
        return View("~/Views/Pages/MiscError.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, exception = ex });
      }

      //var steps = new List<StepModel>
      //{
      //    new StepModel { StepId = 1, Title = "Account Details", Fields = new[] { "Username", "Email", "Password", "ConfirmPassword" } },
      //    new StepModel { StepId = 2, Title = "Personal Info", Fields = new[] { "FirstName", "LastName", "Country", "Language" } },
      //    new StepModel { StepId = 3, Title = "Social Links", Fields = new[] { "Twitter", "Facebook", "LinkedIn" } }
      //};

      //ViewData["Steps"] = steps;
      //return View();
    }

  }
}
