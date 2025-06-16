using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using AspnetCoreMvcFull.Controllers;
using KOAHome.Services;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Newtonsoft.Json;
using KOAHome.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace KOAHome.Controllers
{
  public class NotificationsController : Controller
  {
    private readonly ILogger<NotificationsController> _logger;
    private readonly QLKCL_NEWContext _db;
    private readonly IReportEditorService _re;
    private readonly IAttachmentService _att;
    private readonly IReportService _report;
    private readonly IFormService _form;
    private readonly IActionService _action;
    private readonly IWidgetService _widget;

    public NotificationsController(QLKCL_NEWContext db, ILogger<NotificationsController> logger, IReportEditorService re, IAttachmentService att, IReportService report, IFormService form, IActionService action, IWidgetService widget)
    {
      _db = db;
      _logger = logger;
      _re = re;
      _att = att;
      _report = report;
      _form = form;
      _action = action;
      _widget = widget;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
      Dictionary<string, object> objParameters = new Dictionary<string, object>();
      // lay danh sach thong bao

      var stopwatch = Stopwatch.StartNew();
      var notifications = await _widget.Widget_GetList(objParameters, "HS_Notify_Search", null);
      stopwatch.Stop();
      _logger.LogInformation($"Query notifications executed in {stopwatch.ElapsedMilliseconds} ms");
      // tra ve json
      return Ok(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
      var notification = _db.HsServiceHistories.FirstOrDefault(n => n.Id == id);
      if (notification != null)
      {
        notification.IsRead = true;
      }
      await _db.SaveChangesAsync();
      return Json(new { success = true });
    }

  }
}
