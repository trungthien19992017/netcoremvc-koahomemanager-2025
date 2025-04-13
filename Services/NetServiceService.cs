
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace KOAHome.Services
{
  public interface INetServiceService
  {
    // xu ly lay du lieu tra ve tu service
    public Task<List<SelectListItem>> NET_Service_DynamicExecute(int serviceId, Dictionary<string, object>? parameters);
    // xu ly lay danh sach selectlist tu filter truyen vao
    public Task<Dictionary<string, List<SelectListItem>>> NET_Service_GetListSelectListByFilter(List<dynamic>? filterList, Dictionary<string, object>? objParameters);
  }
  public class NetServiceService : INetServiceService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IConnectionService _con;
    private readonly IMemoryCache _cache;
    private readonly ILogger<NetService> _logger;
    public NetServiceService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IConnectionService con, IMemoryCache cache, ILogger<NetService> logger)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
      _con = con;
      _cache = cache;
      _logger = logger;
    }

    public async Task<List<SelectListItem>> NET_Service_DynamicExecute(int serviceId, Dictionary<string, object>? parameters)
    {
        // su dung datasource config de lay du lieu
        string connectionString = _configuration.GetConnectionString("ConfigConnection"); // Thay thế bằng chuỗi kết nối của bạn
                                                                                          // store get du lieu
        string sqlStore = "NET_Service_DynamicExecute";
        // neu parameter rong thi tu tao 1 parameter moi truyen vao
        if (parameters == null)
        {
          parameters = new Dictionary<string, object>();
        }

        // chuyen tat ca param dang co thanh 1 chuoi json va truyen vao bien Param
        var displayParameter = new Dictionary<string, object>();
        if (!parameters.ContainsKey("Param"))
        {
            parameters.Add("Param", JsonConvert.SerializeObject(parameters));
        }
        parameters.Add("ServiceId", serviceId);

        // chuyen thanh cau query tu store va param truyen vao
        var (sqlQuery, sqlParams) = await _con.Connection_GetQueryParam(parameters, sqlStore, connectionString);

        var resultList = new List<dynamic>();

        // xu ly lay du lieu dua truyen store va param truyen vao
        resultList = await _con.Connection_GetDataFromQuery(parameters, sqlStore, connectionString, sqlQuery, sqlParams);

        // xoa param serviceid ngay de tranh loi phat sinh
        if (parameters.ContainsKey("ServiceId"))
        {
          parameters.Remove("ServiceId");
        }

      List<SelectListItem> listItems = resultList
          .Select(x => new SelectListItem
          {
            Value = x.Id.ToString(),    // hoặc x.GetType().GetProperty("Id") nếu không chắc
            Text = x.Name.ToString()
          })
          .ToList();

      return listItems;
    }

    public async Task<Dictionary<string, List<SelectListItem>>> NET_Service_GetListSelectListByFilter(List<dynamic>? filterList, Dictionary<string, object>? objParameters)
    {
      // doi voi cac filter co kieu select (select box, dropdownbox, tagbox,...), day cac bo select vao SelectListItem va đóng gói trong Dictionary để xử lý trên giao diện
      // Tạo Dictionary chứa SelectList cho từng dropdown (theo DynamicFieldName)
      var listFilterService = new Dictionary<string, List<SelectListItem>>();

      if (filterList != null)
      {
        // 1. Khởi tạo danh sách Task chính xác
        var serviceTasks = filterList
            .Where(f => f.SeviceId != null)
            .Select(async filter =>
            {
              var serviceId = (int)filter.SeviceId!;
              var code = filter.Code!;

              // Serialize param để tạo cache key
              string paramKey = string.Join(";", objParameters
                  .OrderBy(kvp => kvp.Key)
                  .Select(kvp => $"{kvp.Key}={kvp.Value}"));

              string cacheKey = $"Service_{serviceId}_{paramKey.GetHashCode()}";

              List<SelectListItem> selectList;

              var sw = Stopwatch.StartNew();

              // Kiểm tra cache trước
              if (!_cache.TryGetValue(cacheKey, out selectList))
              {
                _logger.LogInformation($"⏳ Đang gọi service {serviceId} cho filter '{code}'");

                // Tạo bản sao của objParameters cho mỗi filter de tranh ghi de
                var clonedParameters = new Dictionary<string, object>(objParameters);
                selectList = await NET_Service_DynamicExecute(serviceId, clonedParameters);

                // Lưu cache trong 5 phút (có thể tuỳ chỉnh)
                _cache.Set(cacheKey, selectList, TimeSpan.FromMinutes(5));
                _logger.LogInformation($"✅ Service {serviceId} filter '{code}' hoàn tất sau {sw.ElapsedMilliseconds}ms (không dùng cache)");
              }
              else
              {
                _logger.LogInformation($"⚡ Service {serviceId} filter '{code}' dùng cache sau {sw.ElapsedMilliseconds}ms");
              }
              sw.Stop();


              return (Code: code, SelectList: selectList); // 👈 đây là fix
            })
            .ToList();

        // 2. Chạy tất cả task song song
        var serviceResults = await Task.WhenAll(serviceTasks);

        // 3. Chuyển kết quả sang Dictionary<string, List<SelectListItem>>
        listFilterService = serviceResults.ToDictionary(x => (string)x.Code, x => x.SelectList);
      }

      return listFilterService;
    }

  }
}
