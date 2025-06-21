
using KOAHome.EntityFramework;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IAttachmentService
  {
    public Task<Dictionary<string, List<string>>> UpdateFiles(IFormCollection form);
    public Task<object> SaveAttachmentTable(IFormCollection form, int Id);
    public Task<Dictionary<string, List<string>>> GetFiles(int? Id, List<string> ListObjectTypeCode);
    public Task<Dictionary<string, List<string>>> HandleFiles(string objectTypeCodes, IFormCollection? form, int? id);

  }
  public class AttachmentService : IAttachmentService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private const string CartSession = "CartSession";
    public AttachmentService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
    }
    public async Task<Dictionary<string, List<string>>> UpdateFiles(IFormCollection form)
    {
      var result = new Dictionary<string, List<string>>();

      if (form.Files.Any())
      {
        foreach (var file in form.Files)
        {
          if (file.Length > 0)
          {
            // Lấy name của input file từ file đầu tiên (chỉ lấy 1 lần)
            string? objectTypeCode = file.Name;
            var folder = Path.Combine("AttachmentFiles", "FORM", objectTypeCode, DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            var uploadFolder = Path.Combine("wwwroot", folder);

            // Kiểm tra và tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(uploadFolder))
            {
              Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, file.FileName);
            // Kiểm tra và xóa file cũ nếu cần
            if (System.IO.File.Exists(filePath))
            {
              System.IO.File.Delete(filePath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
              file.CopyTo(stream);
            }

            var fileUrl = $"/{folder.Replace("\\", "/")}/{file.FileName}";

            if (!result.ContainsKey(objectTypeCode))
            {
              result[objectTypeCode] = new List<string>();
            }

            result[objectTypeCode].Add(fileUrl);
          }
        }
      }

      return result;
    }

    public async Task<object> SaveAttachmentTable(IFormCollection form, int Id)
    {
      if (form.Files.Any())
      {
        // chuyen thong tin file vao paramerter
        // Lấy danh sách tên file
        var fileInfos = form.Files.Select(f => new
        {
          SyntaxCode = f.Name,
          FileName = f.FileName,
          ContentType = f.ContentType
        }).ToList();

        // Chuyển danh sách thành chuỗi JSON
        string fileInfosJson = JsonConvert.SerializeObject(fileInfos);

        // Dictionary chứa các tham số
        var parameters = new Dictionary<string, object>
      {
          { "id", Id},
          { "fileinfosjson", fileInfosJson ?? (object)DBNull.Value }
      };


        var connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
        //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
        string sqlStore = "net_attachment_savefile";

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao
        resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

        //kiem tra du lieu id tra ve
        var ids_return = resultList
        .Where(item => ((IDictionary<string, object>)item).ContainsKey("id"))
        .Select(item => ((IDictionary<string, object>)item)["id"])
        .FirstOrDefault(); // Lọc ra những phần tử có Id

        // neu co gia tri tra ve thi bao thanh cong
        if (ids_return != null)
        {
          string listidStr = ids_return.ToString();

          if (!string.IsNullOrWhiteSpace(listidStr))
          {
            // Trả về kiểu object để controller serialize thành JsonResult
            return new
            {
              success = true,
              listAttachmentId = listidStr
            };
          }
        }
        return new { success = false, errorMessage = "Lưu file không thành công" };
      }
      // Trả về kiểu object để controller serialize thành JsonResult
      return new
      {
        success = true
      };
    }

    public async Task<Dictionary<string, List<string>>> GetFiles(int? Id, List<string> ListObjectTypeCode)
    {
      var connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
      string sqlStore = "net_attachment_getfile";

      var fileUrls = new Dictionary<string, List<string>>();

      if (ListObjectTypeCode != null)
      {
        // 1. Khởi tạo danh sách Task chính xác
        var fileTasks = ListObjectTypeCode
            .Select(async p =>
            {
              var syntaxCode = p.ToString();      // Dictionary chứa các tham số
              // gán giá trị vào param
              var parameters = new Dictionary<string, object>
              {
                  { "objectid", Id},
                  { "objecttypecode", syntaxCode}
              };
              // chuyen thanh cau query tu store va param truyen vao
              var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

              var resultList = new List<dynamic>();
              // xu ly lay du lieu dua truyen store va param truyen vao
              resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

              List<string> listfilename = resultList.Select(item => ((IDictionary<string, object>)item)["filename"].ToString()).ToList();
              return (SyntaxCode: syntaxCode, Listfilename: listfilename); // 👈 đây là fix
            })
            .ToList();

        // 2. Chạy tất cả task song song
        var fileResults = await Task.WhenAll(fileTasks);

        // 3. Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        fileUrls = fileResults.ToDictionary(x => (string)x.SyntaxCode, x => x.Listfilename);
      }

      // nhan du lieu duoi dang object
      return fileUrls;
    }

    public async Task<Dictionary<string,List<string>>> HandleFiles(string objectTypeCodes, IFormCollection? form, int? id)
    {
      var listAttFileUrls = new Dictionary<string, List<string>>();
      // neu co bat ky object type code nào thì tiếp tục
      if (objectTypeCodes != "")
      {
        List<string> listObjectTypeCode = objectTypeCodes.Split(',').ToList();

        if (form != null)
        {
          if (form.Files.Any())
          {
            listAttFileUrls = await UpdateFiles(form); // Gọi service để lưu file
          }
          else if (id != null)
          {
            listAttFileUrls = await GetFiles(id, listObjectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode

          }
        }
        else if (id != null)
        {
          listAttFileUrls = await GetFiles(id, listObjectTypeCode); // Gọi service để get file tu objectId va ObjectTypeCode
        }
      }
      return listAttFileUrls;
    }

  }
}
