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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace KOAHome.Controllers
{
    public class HsBookingsController : Controller
  {
    private readonly ILogger<HsBookingsController> _logger;
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

    public HsBookingsController(QLKCL_NEWContext db, ILogger<HsBookingsController> logger, IHsBookingTableService book, IHsBookingServiceService bookser, IReportEditorService re, IAttachmentService att, IHsCustomerService cus, IReportService report, IFormService form, IActionService action, IWidgetService widget)
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
    }

    // cac phuong thuc con de toi uu xu ly
    private async Task HandleFiles(string objectTypeCode, IFormCollection? form, int? id)
    {
      if (form != null)
      {
        if (form.Files.Any())
        {
          List<string> fileUrls = await _att.UpdateFiles(form); // Gọi service để lưu file
          ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)
        }
        else if (id != null)
        {
          List<string> fileUrls = await _att.GetFiles(id, objectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode
          ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)

        }
      }
      else if (id != null)
      {
        List<string> fileUrls = await _att.GetFiles(id, objectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode
        ViewBag.fileUrls = fileUrls; // Truyền danh sách file về View (neu co)
      }
    }

    private bool CheckForErrors(List<dynamic> resultList, out string errorMessage)
    {
      var errorMessages = resultList
          .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage"))
          .Select(item => item.ErrorMessage.ToString())
          .ToList();

      if (errorMessages.Any())
      {
        errorMessage = string.Join(", ", errorMessages);
        return true;
      }

      errorMessage = null;
      return false;
    }

    // GET: HsBookings
    public async Task<IActionResult> Index([FromQuery] Dictionary<string, string> parameters, int page = 1, int pageSize = 20)
    {
      try
      {
        // Kiểm tra xem có key "isActive" không, nếu không có thì gán giá trị mặc định (null hoặc giá trị khác)
        string isActive = parameters.ContainsKey("isActive") ? parameters["isActive"] : "";

        // Service cho filter hoat dong
        ViewBag.IsActiveOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Có" , Selected = isActive == "1"},
            new SelectListItem { Value = "2", Text = "Không" , Selected = isActive == "2"}
        };

        // chuyen parameters thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        //Phân trang
        // search
        //if (!_memoryCache.TryGetValue("ProductList", out Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> (resultList, totalRecord, maxPage, totalPage)))
        //{
        //  products = await _context.Products.ToListAsync();
        //  _memoryCache.Set("ProductList", products, TimeSpan.FromMinutes(10));
        //}
        var (resultList, totalRecord, maxPage, totalPage) = await _report.Report_Pagination_search("HS_Booking1_search", null, objParameters, page, pageSize);
        ViewBag.listbook_store = resultList;
        // gia tri pham trang tra ve
        ViewBag.Total = totalRecord;
        ViewBag.Page = page;
        ViewBag.TotalPage = totalPage;
        ViewBag.MaxPage = maxPage;
        ViewBag.First = 1;
        ViewBag.Last = totalPage;
        ViewBag.Next = page + 1;
        ViewBag.Prev = page - 1;

        // khai bao service lien quan
        var service_parameters = new Dictionary<string, object>();
        ViewBag.listbookser_info_store = await _report.Report_search(service_parameters, "HS_BookingService_Info", null);

        //khai bao success
        ViewBag.success = "Thành công";


        return View();
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("Error");
      }
    }

    // GET: HsBookings/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            // link qua link edit nhung chi doc
            return RedirectToAction("Edit", new { id = id, isReadOnly = true });
        }

    [HttpGet]
    public async Task<IActionResult> GetDataFillSelection(string value, string key, string datafillstore)
    {
      // param truyen vao
      var parameters = new Dictionary<string, object>
                {
                    { key, value }
                    // Thêm các tham số khác nếu cần
                };

      //xu ly tra ve data fill tu store
      var data = await _form.Form_GetDataFill_FromSelection(parameters, datafillstore, null);

      Console.WriteLine($"Type of data: {data?.GetType()}"); // Kiểm tra kiểu dữ liệu

      return Ok(data);
    }


    //// GET: HsBookings/Create
    public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(
                                          _db.HsCustomers.Select(c => new
                                          {
                                            CustomerId = c.CustomerId,
                                            DisplayText = c.LastName + " - " + c.PhoneNumber
                                          }),
                                          "CustomerId",
                                          "DisplayText"
                                      );
            ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name");
            ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p=>p.IsActive == true).OrderBy(p=>p.OrderId), "ServiceId", "Name");
            ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");

            //khai bao phan tu chua data
            dynamic dynamicModel = new ExpandoObject();
            var formData = (IDictionary<string, object>)dynamicModel;
            ViewBag.formData = formData;

            return View();
        }

        // POST: HsBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] IFormCollection form)
        {
            // du lieu show tren giao dien
            ViewData["CustomerId"] = new SelectList(
                                          _db.HsCustomers.Select(c => new 
                                          {
                                              CustomerId = c.CustomerId, 
                                              DisplayText = c.LastName + " - " + c.PhoneNumber 
                                          }),
                                          "CustomerId",
                                          "DisplayText",
                                          form["CustomerID"]
                                      );
            ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name", form["RoomID"]);
            ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");
            ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");
            // danh sach service theo booking (khong co du lieu)
            var bookSerResultList = await _report.ReportDetail_FromParent("BookingID",0.ToString(), "HS_BookingService_search", null);
            ViewBag.listbookserbybookingid_store = bookSerResultList;

            // xu ly file
            // Kiểm tra xem form có file nào không? Nếu có thì cập nhật file sau đó trả về danh sách file
            await HandleFiles("BookingFile",form,null);

            // Convert the IFormCollection to a dictionary of strings
            var formData = form.ToDictionary(
                            pair => pair.Key,
                            pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                        );

            // gui form data len view de hien thi
            ViewBag.formData = formData;

            // xu ly luu form
            var resultList = await _form.Form_ups(formData,null,"HS_Booking1_ups", null);
            //kiem tra du lieu id tra ve
            var id_return = resultList
            .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
            .Select(item => ((IDictionary<string, object>)item)["Id"])
            .FirstOrDefault(); // Lọc ra những phần tử có Id

            // neu co gia tri tra ve thi bao thanh cong
            if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
            {
              int id = (int)id_return;
              // xu ly luu bang attachment
              bool isSaveAttachment = await _att.SaveAttachmentTable(form, id);
              if (!isSaveAttachment)
              {
                ViewBag.ErrorMessage = "Lưu file không thành công";
              }
        
              // xu ly file
              // Kiểm tra xem form có file nào không
              await HandleFiles("BookingFile",null,id);

              //xu ly report form
              // Dictionary để nhóm dữ liệu theo số thứ tự [n]
              // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
              string reportJsonData = await _re.ExtractGridDataToJson(form);
              //end xu ly report form
              var reportResultList = await _re.ReportEditor_Json_Update(id, reportJsonData, "HS_BookingService_Json_ups", null);
              //kiem tra ton tai error message tu result list tra ve
              // Kiểm tra và nối giá trị của ErrorMessage
              // Nếu có ErrorMessages, nối chúng lại thành một chuỗi
              if (CheckForErrors(reportResultList, out string errorMessage))
              {
                ViewBag.ErrorMessage = errorMessage;
                return View();
              }
              else
              {
                // neu thanh cong thi load du lieu
                ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
                return RedirectToAction("Index");
              }
            }
            else
            {
              //kiem tra ton tai error message tu result list tra ve
              // Kiểm tra và nối giá trị của ErrorMessage
              // Nếu có ErrorMessages, nối chúng lại thành một chuỗi
              if (CheckForErrors(resultList, out string errorMessage))
              {
                  ViewBag.ErrorMessage = errorMessage;
                  return View();
              }
              // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
              else
              {
                  ViewBag.ErrorMessage = "Chưa trả về Id"; 
                  return View();
              }
            }
        }

        // GET: HsBookings/Edit/5
        public async Task<IActionResult> Edit(int? id, bool isReadOnly = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            //khai bao phan tu chua data
            var formData = await _form.Form_sel(id, "HS_Booking1_sel", null);
            ViewBag.formData = formData;


            // xu ly cac du lieu select box va radio tren giao dien
            ViewData["CustomerId"] = new SelectList(
                                          _db.HsCustomers.Select(c => new
                                          {
                                            CustomerId = c.CustomerId,
                                            DisplayText = c.LastName + " - " + c.PhoneNumber
                                          }),
                                          "CustomerId",
                                          "DisplayText",
                                          (formData.ContainsKey("CustomerID") ? formData["CustomerID"] : "")
                                      );
            ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name", (formData.ContainsKey("RoomID") ? formData["RoomID"] : ""));
            ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");
            ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");

            // xu ly file
            // Kiểm tra xem form có file nào không
            await HandleFiles("BookingFile",null,id);
      
            // danh sach service theo booking 
            var bookSerResultList = await _report.ReportDetail_FromParent("BookingID", (id ?? 0).ToString(), "HS_BookingService_search", null);
            ViewBag.listbookserbybookingid_store = bookSerResultList;

            // set readonly form neu co isreadonly = false
            ViewBag.IsReadOnly = isReadOnly;

            return View();
        }

        // POST: HsBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] IFormCollection form)
        {
        // du lieu show tren giao dien
        ViewData["CustomerId"] = new SelectList(
                                      _db.HsCustomers.Select(c => new
                                      {
                                        CustomerId = c.CustomerId,
                                        DisplayText = c.LastName + " - " + c.PhoneNumber
                                      }),
                                      "CustomerId",
                                      "DisplayText",
                                      form["CustomerID"]
                                  );
        ViewData["RoomId"] = new SelectList(_db.HsRooms, "RoomId", "Name", form["RoomID"]);
        ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");
        ViewData["Gender"] = new SelectList(_db.CategoryDetails.Where(p => p.CategoryCode == "Gender"), "Id", "Name");
        // danh sach service theo booking (khong co du lieu)
        var bookSerResultList = await _report.ReportDetail_FromParent("BookingID",id.ToString(), "HS_BookingService_search", null);
        ViewBag.listbookserbybookingid_store = bookSerResultList;

        // xu ly file
        // Kiểm tra xem form có file nào không
        await HandleFiles("BookingFile",form,id);

              // Convert the IFormCollection to a dictionary of strings
              var formData = form.ToDictionary(
                              pair => pair.Key,
                              pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                          );

              // gui form data len view de hien thi
              ViewBag.formData = formData;

              var resultList = await _form.Form_ups(formData, id, "HS_Booking1_ups", null);
              //kiem tra du lieu id tra ve
              var id_return = resultList
              .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
              .Select(item => ((IDictionary<string, object>)item)["Id"])
              .FirstOrDefault(); // Lọc ra những phần tử có Id

              // neu co gia tri tra ve thi bao thanh cong
              if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
              {
                id = (int)id_return;
        
                // xu ly luu bang attachment
                bool isSaveAttachment = await _att.SaveAttachmentTable(form, id);
        
                // xu ly file
                // Kiểm tra xem form có file nào không
                await HandleFiles("BookingFile",null,id);

                if (!isSaveAttachment)
                {
                  ViewBag.ErrorMessage = "Lưu file không thành công";
                }

                //xu ly report form
                // Dictionary để nhóm dữ liệu theo số thứ tự [n]
                // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
                string reportJsonData = await _re.ExtractGridDataToJson(form);
                //end xu ly report form
                var reportResultList = await _re.ReportEditor_Json_Update(id, reportJsonData, "HS_BookingService_Json_ups", null);
                //kiem tra ton tai error message
                // Kiểm tra và nối giá trị của ErrorMessage
                if (CheckForErrors(reportResultList, out string errorMessage))
                {
                    ViewBag.ErrorMessage = errorMessage;
                    return View();
                }
                // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
                else
                {
                  
                  bookSerResultList = await _report.ReportDetail_FromParent("BookingID", id.ToString(), "HS_BookingService_search", null);
                  ViewBag.listbookserbybookingid_store = bookSerResultList;
                  //khai bao phan tu chua data
                  formData = (Dictionary<string, object>)await _form.Form_sel(id, "HS_Booking1_sel", null);
                  ViewBag.formData = formData;

                  ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
                  return View();
                }
              }
              else
              {
                //kiem tra ton tai error message
                // Kiểm tra và nối giá trị của ErrorMessage
                if (CheckForErrors(resultList, out string errorMessage))
                {
                    ViewBag.ErrorMessage = errorMessage;
                    return View();
                }
                // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
                else
                {
                    ViewBag.ErrorMessage = "Chưa trả về Id"; 
                    return View();
                }
              }
        }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> XacNhanThanhToan(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      //khai bao phan tu chua data
      var formData = await _form.Form_sel(id, "HS_Booking1_Payment_sel", null);
      ViewBag.formData = formData;

      return View();
    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhanThanhToan(int id, [FromForm] IFormCollection form)
    {

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      var resultList = await _form.Form_ups(formData, id, "HS_Booking1_Payment_ups", null);
      //kiem tra du lieu id tra ve
      var id_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
      .Select(item => ((IDictionary<string, object>)item)["Id"])
      .FirstOrDefault(); // Lọc ra những phần tử có Id

      // neu co gia tri tra ve thi bao thanh cong
      if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
      {
        id = (int)id_return;

        //khai bao phan tu chua data
        formData = (Dictionary<string, object>)await _form.Form_sel(id, "HS_Booking1_Payment_sel", null);
        ViewBag.formData = formData;

        ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
        return View();
        
      }
      else
      {
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (CheckForErrors(resultList, out string errorMessage))
        {
          ViewBag.ErrorMessage = errorMessage;
          return View();
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          ViewBag.ErrorMessage = "Chưa trả về Id";
          return View();
        }
      }
    }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> GiamGiaPhatSinh(string value, string key, string datafillstore)
    {
      int id = int.TryParse(value, out int parsedId) ? parsedId : 0;

      //khai bao phan tu chua data
      var formData = await _form.Form_sel(id, "HS_Booking1_OtherDiscount_sel", null);
      ViewBag.formData = formData;

      // xu ly cac du lieu select box va radio tren giao dien
      var ListCustomerID = _db.HsCustomers.Select(c => new
                          {
                            id = c.CustomerId,
                            name = c.LastName + " - " + c.PhoneNumber
                          }).ToList();

      // add select box, radio,... vào formData
      formData.Add("ListCustomerID", ListCustomerID);

      return Ok(formData);
    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> GiamGiaPhatSinh(int? id, [FromForm] IFormCollection form)
    {
      
      // du lieu show tren giao dien
      ViewData["CustomerId"] = new SelectList(
                                    _db.HsCustomers.Select(c => new
                                    {
                                      CustomerId = c.CustomerId,
                                      DisplayText = c.LastName + " - " + c.PhoneNumber
                                    }),
                                    "CustomerId",
                                    "DisplayText",
                                    form["CustomerID"]
                                );

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      var resultList = await _form.Form_ups(formData, id, "HS_Booking1_OtherDiscount_ups", null);
      //kiem tra du lieu id tra ve
      var id_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
      .Select(item => ((IDictionary<string, object>)item)["Id"])
      .FirstOrDefault(); // Lọc ra những phần tử có Id

      // neu co gia tri tra ve thi bao thanh cong
      if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
      {
        id = (int)id_return;

        //khai bao phan tu chua data
        formData = (Dictionary<string, object>) await _form.Form_sel(id, "HS_Booking1_OtherDiscount_sel", null);
        ViewBag.formData = formData;

        ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
        return Json(new { success = true });

      }
      else
      {
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        if (CheckForErrors(resultList, out string errorMessage))
        {
          ViewBag.ErrorMessage = errorMessage;
          return View();
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          return Json(new { success = false, errorMessage = "Chưa trả về Id" });
        }
      }
    }

    // GET: HsBookings/Edit/5
    public async Task<IActionResult> DichVuPhatSinh(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      //khai bao phan tu chua data
      var formData = await _form.Form_sel(id, "HS_Booking1_DichVuPhatSinh_sel", null);
      ViewBag.formData = formData;

      // danh sach service theo booking 
      var bookSerResultList = await _report.ReportDetail_FromParent("BookingID", (id ?? 0).ToString(), "HS_BookingService_search", null);
      ViewBag.listbookserbybookingid_store = bookSerResultList;

      // xu ly cac du lieu select box va radio tren giao dien
      ViewData["CustomerId"] = new SelectList(
                                    _db.HsCustomers.Select(c => new
                                    {
                                      CustomerId = c.CustomerId,
                                      DisplayText = c.LastName + " - " + c.PhoneNumber
                                    }),
                                    "CustomerId",
                                    "DisplayText",
                                    (formData.ContainsKey("CustomerID") ? formData["CustomerID"] : "")
                                );
      ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");

      return View();
    }

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DichVuPhatSinh(int id, [FromForm] IFormCollection form)
    {

      // du lieu show tren giao dien
      ViewData["CustomerId"] = new SelectList(
                                    _db.HsCustomers.Select(c => new
                                    {
                                      CustomerId = c.CustomerId,
                                      DisplayText = c.LastName + " - " + c.PhoneNumber
                                    }),
                                    "CustomerId",
                                    "DisplayText",
                                    form["CustomerID"]
                                );
      ViewData["ServiceId"] = new SelectList(_db.HsServices.Where(p => p.IsActive == true).OrderBy(p => p.OrderId), "ServiceId", "Name");

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );        // danh sach service theo booking (khong co du lieu)
      var bookSerResultList = await _report.ReportDetail_FromParent("BookingID", id.ToString(), "HS_BookingService_search", null);
      ViewBag.listbookserbybookingid_store = bookSerResultList;

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      // luu editor truoc
      //xu ly report form
      // Dictionary để nhóm dữ liệu theo số thứ tự [n]
      // Chuyển đổi dữ liệu sang JSON (loc du lieu form tra ve lay du lieu grid va chuyen thanh json)
      string reportJsonData = await _re.ExtractGridDataToJson(form);
      //end xu ly report form
      var reportResultList = await _re.ReportEditor_Json_Update(id, reportJsonData, "HS_BookingService_PhatSinh_Json_ups", null);
      //kiem tra ton tai error message
      // Kiểm tra và nối giá trị của ErrorMessage
      if (CheckForErrors(reportResultList, out string errorMessage))
      {
        ViewBag.ErrorMessage = errorMessage;
        return View();
      }
      // thanh cong thi tiep tuc
      else
      {
        var resultList = await _form.Form_ups(formData, id, "HS_Booking1_DichVuPhatSinh_ups", null);
        //kiem tra du lieu id tra ve
        var id_return = resultList
        .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
        .Select(item => ((IDictionary<string, object>)item)["Id"])
        .FirstOrDefault(); // Lọc ra những phần tử có Id

        // neu co gia tri tra ve thi bao thanh cong
        if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
        {
          id = (int)id_return;

          bookSerResultList = await _report.ReportDetail_FromParent("BookingID", id.ToString(), "HS_BookingService_search", null);
          ViewBag.listbookserbybookingid_store = bookSerResultList;
          //khai bao phan tu chua data
          formData = (Dictionary<string, object>)await _form.Form_sel(id, "HS_Booking1_DichVuPhatSinh_sel", null);
          ViewBag.formData = formData;

          ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
          return View();
        }
        else
        {
          //kiem tra ton tai error message
          // Kiểm tra và nối giá trị của ErrorMessage
          if (CheckForErrors(resultList, out string errorMessage1))
          {
            ViewBag.ErrorMessage = errorMessage1;
            return View();
          }
          // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
          else
          {
            ViewBag.ErrorMessage = "Chưa trả về Id";
            return View();
          }
        }
      }
    }

    //// GET: HsBookings/Edit/5
    //public async Task<IActionResult> HuyPhong(int? id)
    //{
    //  if (id == null)
    //  {
    //    return NotFound();
    //  }

    //  //khai bao phan tu chua data
    //  var formData = await _form.Form_sel(id, "HS_Booking1_Cancel_sel", null);
    //  ViewBag.formData = formData;

    //  return View();
    //}

    // POST: HsBookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> HuyPhong(int id, [FromForm] IFormCollection form)
    {

      // Convert the IFormCollection to a dictionary of strings
      var formData = form.ToDictionary(
                      pair => pair.Key,
                      pair => (object)pair.Value.ToString()  // Ensure each value is a string (flatten StringValues)
                  );

      // gui form data len view de hien thi
      ViewBag.formData = formData;

      var resultList = await _form.Form_ups(formData, id, "HS_Booking1_Cancel_ups", null);
      //kiem tra du lieu id tra ve
      var id_return = resultList
      .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
      .Select(item => ((IDictionary<string, object>)item)["Id"])
      .FirstOrDefault(); // Lọc ra những phần tử có Id

      // neu co gia tri tra ve thi bao thanh cong
      if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
      {
        id = (int)id_return;

        //khai bao phan tu chua data
        formData = (Dictionary<string, object>)await _form.Form_sel(id, "HS_Booking1_Cancel_sel", null);
        ViewBag.formData = formData;

        ViewBag.success = "Xử lý thành công"; // Gán vào ViewBag
        return Json(new { success = true });

      }
      else
      {
        //kiem tra ton tai error message
        // Kiểm tra và nối giá trị của ErrorMessage
        var errorMessages = resultList
                    .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage")) // Lọc ra những phần tử có ErrorMessage
                    .Select(item => item.ErrorMessage.ToString()) // Chọn trường ErrorMessage
                    .ToList(); // Chuyển thành danh sách

        // Nếu có ErrorMessages, nối chúng lại thành một chuỗi
        if (errorMessages.Any())
        {
          string aggregatedErrors = string.Join(", ", errorMessages); // Nối các lỗi lại thành một chuỗi
          ViewBag.ErrorMessage = aggregatedErrors; // Gán vào ViewBag
          return Json(new { success = false, errorMessage = aggregatedErrors });
        }
        // khong tra ve Id, cung khong tra ve error message thi bao loi chua tra ve id
        else
        {
          ViewBag.ErrorMessage = "Chưa trả về Id";
          return Json(new { success = false, errorMessage = "Chưa trả về Id" });
        }
      }
    }

    //  // POST: HsBookings/Delete/5
    //  [HttpPost]
    //  public JsonResult DeleteConfirmed(int id)
    //  {
    //  try
    //  {
    //    // xu ly luu form
    //    var resultList = _action.Action_store(id, null, "HS_Booking1_del", null);
    //    //kiem tra du lieu success tra ve
    //    var success_return = resultList
    //    .Where(item => ((IDictionary<string, object>)item).ContainsKey("Success"))
    //    .Select(item => ((IDictionary<string, object>)item)["Success"])
    //    .FirstOrDefault(); // Lọc ra những phần tử có Success

    //    //kiem tra du lieu error message tra ve
    //    var errormessage_return = resultList
    //    .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage"))
    //    .Select(item => ((IDictionary<string, object>)item)["ErrorMessage"])
    //    .FirstOrDefault(); // Lọc ra những phần tử có ErrorMessage

    //    bool success = false;
    //    string? errorMessage = null;
    //    // neu co gia tri success tra ve thi xu ly tiep, khong thi bao loi
    //    if (success_return != null && bool.TryParse(success_return.ToString(), out bool issuccess))
    //    {
    //      success = (bool)success_return;
    //      // kiem tra co error message tra ve hay khong
    //      if (errormessage_return != null && errormessage_return.ToString() != "")
    //      {
    //        errorMessage = errormessage_return.ToString();
    //      }
    //      // tra ve json
    //      if (success == true)
    //      {
    //        return Json(new { success = true });
    //      }
    //      else
    //      {
    //        return Json(new { success = false, errorMessage = errorMessage ?? "Có lỗi trong quá trình xử lý" });
    //      }
    //    }
    //    else
    //    {
    //      return Json(new { success = false, errorMessage = "Store chưa trả về giá trị success." });
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    return Json(new { success = false, errorMessage = ex.Message });
    //  }
    //}

    // GET: HsBookings/LichBookingThang
    public async Task<IActionResult> LichBookingThang([FromQuery] Dictionary<string, string> parameters)
    {
      try
      {
        // Kiểm tra xem có key "LocNhanh" không, nếu không có thì gán giá trị mặc định (null hoặc giá trị khác)
        string locnhanh = parameters.ContainsKey("LocNhanh") ? parameters["LocNhanh"] : "";

        // Service cho filter hoat dong
        ViewBag.LocNhanhOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "DaThanhToan_LichHomNay", Text = "Có lịch đặt phòng trong hôm nay (Vàng)" , Selected = locnhanh == "DaThanhToan_LichHomNay"},
            new SelectListItem { Value = "DaThanhToan_ChuaToiLich", Text = "Đã thanh toán nhưng chưa tới lịch (Trắng)" , Selected = locnhanh == "DaThanhToan_ChuaToiLich"},
            new SelectListItem { Value = "ChuaThanhToan", Text = "Chưa thanh toán (Đỏ)" , Selected = locnhanh == "ChuaThanhToan"},
            new SelectListItem { Value = "DaThanhToan_DaCheckOut", Text = "Đã hoàn tất check out và thanh toán (Xanh navy)" , Selected = locnhanh == "DaThanhToan_DaCheckOut"}
        };

        // chuyen parameters thanh Idictionary<string, object>
        Dictionary<string, object> objParameters = parameters.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);

        //Phân trang
        // search
        var stopwatch = Stopwatch.StartNew();
        var resultList = await _report.Report_search(objParameters, "HS_LichBookingThang", null);
        stopwatch.Stop();
        _logger.LogInformation($"Query HS_LichBookingThang executed in {stopwatch.ElapsedMilliseconds} ms");

        //param truyen vao de lay danh sach cot
        Dictionary<string, object> displayParameters = new Dictionary<string, object>();
        // cac du lieu parameter se add vao store display duoi dang bien param cach nhau bang dau ;
        string displayParamString = string.Join(";", parameters.Select(kvp => $"{kvp.Key}={kvp.Value ?? ""}"));
        // add param
        displayParameters.Add("Param", displayParamString);
        displayParameters.Add("ReportID", "2208");
        displayParameters.Add("SQLContent", "DRDisplay_HS_LichBookingThang_CustomStore");
        // search danh sach
        //
        // Tiếp tục log thời gian lấy danh sách cột
        stopwatch.Restart();
        var displayList = await _report.Report_search(displayParameters, "Store_DRDisplay", null);
        stopwatch.Stop();
        _logger.LogInformation($"Report_search display executed in {stopwatch.ElapsedMilliseconds} ms");

                // dua vao view bag de chuyen qua view
                ViewBag.resultList = resultList;
        ViewBag.displayList = displayList;
        ViewBag.Total = resultList.Count() > 0 ? resultList.Count() : 0;

        //khai bao success
        ViewBag.success = "Thành công";

        return View();
      }
      catch (Exception ex)
      {
        // Log the exception
        _logger.LogError(ex, "An error occurred while fetching booking service info.");
        // Optionally, return an error view
        return View("Error");
      }
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
      Dictionary<string, object> objParameters = new Dictionary<string, object>();
      //////// Widget List item ty le kin phong trong tuan
      var notifications = await _widget.Widget_GetList(objParameters, "HS_Notify_Search", null);
      return Ok(notifications);
    }

    private bool HsBookingExists(int id)
      {
          return _db.HsBookings.Any(e => e.BookingId == id);
      }
    }
}
