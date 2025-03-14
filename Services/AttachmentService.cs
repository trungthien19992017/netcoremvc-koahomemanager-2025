
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace KOAHome.Services
{
  public interface IAttachmentService
  {
    public Task<List<string>> UpdateFiles(IFormCollection form);
    public Task<bool> SaveAttachmentTable(IFormCollection form, int Id);
    public Task<List<string>> GetFiles(int? Id, string ObjectTypeCode);

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
    public async Task<List<string>> UpdateFiles(IFormCollection form)
    {
      // Lấy name của input file từ file đầu tiên (chỉ lấy 1 lần)
      string? objectTypeCode = form.Files.FirstOrDefault()?.Name;

      List<string> fileUrls = new List<string>();
      if (form.Files.Any())
      {
        var folder = Path.Combine("img", objectTypeCode, DateTime.Now.ToString("ddMMyyyy", CultureInfo.InvariantCulture));
        var uploadFolder = Path.Combine("wwwroot", folder);

        // Kiểm tra và tạo thư mục nếu chưa tồn tại
        if (!Directory.Exists(uploadFolder))
        {
          Directory.CreateDirectory(uploadFolder);
        }

        foreach (var file in form.Files)
        {
          if (file.Length > 0)
          {
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
            fileUrls.Add($"/{folder}/{file.FileName}"); // Lưu đường dẫn file
          }
        }
      }

      return fileUrls;
    }

    public async Task<bool> SaveAttachmentTable(IFormCollection form, int Id)
    {
      if (form.Files.Any())
      {
        // Lấy name của input file từ file đầu tiên (chỉ lấy 1 lần)
        string? objectTypeCode = form.Files.FirstOrDefault()?.Name;
        // chuyen thong tin file vao paramerter
        // Lấy danh sách tên file
        var fileInfos = form.Files.Select(f => new
        {
          FileName = f.FileName,
          ContentType = f.ContentType
        }).ToList();

        // Chuyển danh sách thành chuỗi JSON
        string fileInfosJson = JsonConvert.SerializeObject(fileInfos);

        // Dictionary chứa các tham số
        var parameters = new Dictionary<string, object>
      {
          { "Id", Id},
          { "FileInfosJson", fileInfosJson ?? (object)DBNull.Value },
          { "ObjectTypeCode", objectTypeCode ?? (object)DBNull.Value }
      };


        var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
        //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
        string sqlStore = "HS_Attachment_SaveFile";

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao
        resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

        //kiem tra du lieu id tra ve
        var id_return = resultList
        .Where(item => ((IDictionary<string, object>)item).ContainsKey("Id"))
        .Select(item => ((IDictionary<string, object>)item)["Id"])
        .FirstOrDefault(); // Lọc ra những phần tử có Id

        // neu co gia tri tra ve thi bao thanh cong
        if (id_return != null && int.TryParse(id_return.ToString(), out int num) && num > 0)
        {
          return true;
        }
        return false;
      }
      return true;
    }

    public async Task<List<string>> GetFiles(int? Id, string ObjectTypeCode)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      string sqlStore = "HS_Attachment_GetFile";

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "ObjectId", Id},
          { "ObjectTypeCode", ObjectTypeCode}
      };

      // chuyen thanh cau query tu store va param truyen vao
      var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

      var resultList = new List<dynamic>();

      // xu ly lay du lieu dua truyen store va param truyen vao
      resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);
      List<string> listfilename = resultList.Select(item => ((IDictionary<string, object>)item)["FileName"].ToString()).ToList();

      // nhan du lieu duoi dang object
      return listfilename;
    }

  }
}
