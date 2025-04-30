using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace KOAHome.Controllers
{
  public class NETMenuController : Controller
  {
    private readonly ILogger<NETFormController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IHsBookingTableService _book;
    private readonly IHsBookingServiceService _bookser;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IHsCustomerService _cus;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IActionService _action;
    private readonly IWidgetService _widget;
    private readonly IDRDatasourceService _datasrc;
    private readonly INetServiceService _netService;
    private readonly INetMenuService _menu;
    private readonly IConnectionService _con;


    public NETMenuController(QLKCL_NEWContext db, ILogger<NETFormController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget, IDRDatasourceService datasrc, INetServiceService netService, IConnectionService con, INetMenuService menu)
    {
      _db = db;
      _logger = logger;
      _book = book;
      _bookser = bookser;
      _re = re;
      _att = att;
      _cus = cus;
      _report = report;
      _form = form;
      _action = action;
      _widget = widget;
      _datasrc = datasrc;
      _netService = netService;
      _con = con;
      _menu = menu;
    }

    // GET: NETMenuController
    public ActionResult Index()
    {
      return View();
    }

    // danh sách main menu (gọi ajax)
    [HttpGet]
    public async Task<IActionResult> MainMenu_List()
    {
      try
      {
        // Lấy dynamic query parameters
        var parameters = Request.Query;

        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());
        // đưa popup param qua view để chuyển cho các partial nếu có
        ViewData["objParameters"] = objParameters;

        // lay danh sach main menu, có lọc theo param từ url truyền vào
        var mainMenuList = await _menu.NET_MainMenu_List(objParameters);
        ViewData["mainMenuList"] = mainMenuList;

        ViewData["sucess"] = "Thành công";

        return PartialView("~/Views/Shared/Partial/NETMenu/_MainMenu_Partial.cshtml");
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching form.");
        // Optionally, return an error view
        return View("Error");
      }
    }

    // danh sach menu theo main menu code (gọi ajxa)
    [HttpGet]
    public async Task<IActionResult> Menu_List(string currentPage, string MenuCode = "Management_KOA")
    {
      try
      {
        // chuyển currentPage qua View để kiểm tra
        ViewData["currentPage"] = currentPage;

        // kiểm tra có tồn tại session lưu menucode chưa? nếu có rồi thì lấy từ session, nếu chưa có thì lấy menucode mặc định rồi lưu vào session
        string? currentMenuCode = HttpContext.Session.GetString("CurrentMenuCode");
        if (currentMenuCode != null)
        {
          MenuCode = currentMenuCode;
        }
        else
        {
          HttpContext.Session.SetString("CurrentMenuCode", MenuCode);
        }

        // neu không trả về menu code thì chuyển sang link lỗi
        if (MenuCode == null)
        {
          return Json(new { success = false, errorMessage = "Không tồn tại mã menu" });
        }
        ViewData["MenuCode"] = MenuCode;

        // Lấy dynamic query parameters
        var parameters = Request.Query;
        // chuyen parameters cua duong dan thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.ToString());
        // đưa popup param qua view để chuyển cho các partial nếu có
        ViewData["objParameters"] = objParameters;

        // lấy thông tin main menu
        var mainmenu = await _menu.NET_MainMenu_Get(MenuCode);
        // tra ve page loi neu khong tim thay main menu
        if (mainmenu == null)
        {
          return Json(new { success = false, errorMessage = "Không tìm thấy thông tin main menu" });
        }
        // chuyển thông tin main menu lên view
        ViewData["mainmenu"] = mainmenu;

        // lay thong tin menu, va danh sach main cua main menu de xu ly
        var menuList = await _menu.NET_Menu_WithMainMenu_Get(MenuCode,null);
        // tra ve page loi neu khong tim thay menu
        if (menuList == null)
        {
          return Json(new { success = false, errorMessage = "Không tìm thấy danh sách menu" });
        }
        // chuyen cau hinh menu len giao dien de xu ly
        ViewData["menuList"] = menuList;

        ViewData["sucess"] = "Thành công";

        return PartialView("~/Views/Shared/Partial/NETMenu/_Menu_Partial.cshtml");
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("Error");
      }

    }

    // mục tiêu để gán menucode mặc định vào session
    [HttpPost]
    public IActionResult SetMenuCode(string menuCode)
    {
      if (!string.IsNullOrEmpty(menuCode))
      {
        HttpContext.Session.SetString("CurrentMenuCode", menuCode);
        return Json(new { success = true });
      }

      return Json(new { success = false, errorMessage = "MenuCode không hợp lệ" });
    }

  }
}
