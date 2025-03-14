
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Data;

namespace KOAHome.Services
{
  public interface IHsBookingTableService
  {
    public Task<List<HsBooking>> LoadListBooking();
    public Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> HS_Booking_Search(Dictionary<string, string> parameters, int page = 1, int pageSize = 10);
    public Task<List<dynamic>> HS_Booking_Update(Dictionary<string, object> parameters, int? id);
    public Task<IDictionary<string, object>> HS_Booking_Form_sel(int? Id);
    public Task<List<dynamic>> HS_Booking1_Payment_ups(Dictionary<string, object> parameters, int? id);
    public Task<IDictionary<string, object>> HS_Booking1_Payment_sel(int? Id);

  }
  public class HsBookingTableService : IHsBookingTableService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private const string CartSession = "CartSession";
    public HsBookingTableService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
    }
    public async Task<List<HsBooking>> LoadListBooking()
    {
      return await _db.HsBookings.ToListAsync();
    }
    public async Task<(List<dynamic> Results, int TotalRecord, int MaxPage, int TotalPage)> HS_Booking_Search(Dictionary<string, string> parameters, int page = 1, int pageSize = 10)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
      var sqlQuery = new StringBuilder("EXEC dbo.HS_Booking1_search");

      var sqlParams = new List<SqlParameter>();
      foreach (var param in parameters)
      {
        sqlQuery.Append($" @{param.Key} = @{param.Key},");
        sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value ?? (object)DBNull.Value));
       }

      // Xóa dấu phẩy cuối cùng
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }

      var resultList = new List<dynamic>();
      var totalRecord = 0;
      var maxPage = 0;
      var totalPage = 0;

      using (var connection = new SqlConnection(connectionString))
        {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(sqlParams.ToArray());

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row.Add(reader.GetName(i), reader.GetValue(i));
              }
              resultList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }

      totalRecord = resultList.Count();
      maxPage = 3;
      if (totalRecord % pageSize != 0)
        totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)) + 1;
      else
        totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));


      //Phân trang
      if (resultList.Skip((page - 1) * pageSize).Count() < pageSize)
        resultList = resultList.Skip((page - 1) * pageSize).Take((int)(totalRecord % pageSize)).ToList();
      else
        resultList = resultList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      return (resultList, totalRecord, maxPage, totalPage);
    }
    public async Task<List<dynamic>> HS_Booking_Update(Dictionary<string, object> parameters, int? id)
    {
      // add Id vao paramerter neu co
      if (id != null)
      {
        parameters.Add("Id", id);
      }
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
      string sqlStore = "HS_Booking1_ups";
      var sqlQuery = new StringBuilder("EXEC dbo."+ sqlStore);

      //query store param 
      string paramfromstore = "select parameter_name,data_type from information_schema.parameters where specific_name = '" + sqlStore + "'";

      var paramfromstoreList = new List<dynamic>();
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(paramfromstore, connection))
        {
          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row.Add(reader.GetName(i), reader.GetValue(i));
              }
              paramfromstoreList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }

      //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
      // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
      var sqlParams = new List<SqlParameter>();
      for (int i = 0; i < paramfromstoreList.Count(); i++)
      {
        //param key
        string store_key = ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Substring(1, ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Length - 1);
        //param type
        string store_datatype = ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Substring(0, ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Length);
        //neu form field co chua param trong store thi xu ly tiep
        if (parameters.ContainsKey(store_key))
        {
          if (parameters[store_key] != null && parameters[store_key].ToString() != "")
          {
            object value;
            var check_date = new List<string> { "date", "datetime", "time" };
            var check_num = new List<string> { "int", "bigint", "decimal", "float" };
            //xu ly du lieu truoc khi dua vao store
            if (check_date.Contains(store_datatype))
            {
              DateTime date = Convert.ToDateTime(parameters[store_key]);
              string format = "yyyy-MM-dd HH:mm:ss";
              value = date.ToString(format);
            }
            else if (check_num.Contains(store_datatype))
            {
              double num = Convert.ToDouble(parameters[store_key]);
              value = num;
            }
            //else if (store_datatype == "TagBox" || store_datatype == "DropDownBox")
            //{
            //  var list = JArray.Parse(parameters[store_key].ToString());
            //  string res = list.Count == 0 ? null : string.Join(",", list);
            //  value = res;
            //}
            else
            {
              value = parameters[store_key].ToString();
            }

            sqlQuery.Append($" @{store_key} = @{store_key},");
            sqlParams.Add(new SqlParameter($"@{store_key}", value ?? DBNull.Value));
          }
        }
      }

      //foreach (var param in parameters)
      //{
      //  sqlQuery.Append($" @{param.Key} = @{param.Key},");
      //  sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value ?? DBNull.Value));
      //}

      // Xóa dấu phẩy cuối cùng
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }

      var resultList = new List<dynamic>();
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(sqlParams.ToArray());

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row.Add(reader.GetName(i), reader.GetValue(i));
              }
              resultList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }

      return resultList;
    }

    public async Task<IDictionary<string, object>> HS_Booking_Form_sel(int? Id)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      string sqlStore = "HS_Booking1_sel";
      var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "Id", Id}
      };

      var sqlParams = new List<SqlParameter>();
      foreach (var param in parameters)
      {
        sqlQuery.Append($" @{param.Key} = @{param.Key},");
        sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value));
      }

      // Xóa dấu phẩy cuối cùng
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }

      var resultList = new List<dynamic>();

      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(sqlParams.ToArray());

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                string value = reader.GetValue(i).ToString();
                string key = reader.GetName(i).ToString();
                // Nếu giá trị là chuỗi rỗng, gán `null`
                if (string.IsNullOrEmpty(value))
                {
                  value = null;
                }
                row.Add(key, value);
              }
              resultList.Add(row);
            }
          }
        }
      }
      // Chuyển đổi List<dynamic> thành Dictionary<string, object>
            var dictionary = resultList
          .SelectMany(obj => ((IDictionary<string, object>)obj)
              .Select(prop => new KeyValuePair<string, object>(prop.Key, prop.Value)))
          .ToDictionary(pair => pair.Key, pair => pair.Value);

      // nhan du lieu duoi dang object
      return dictionary;
    }

    public async Task<List<dynamic>> HS_Booking1_Payment_ups(Dictionary<string, object> parameters, int? id)
    {
      // add Id vao paramerter neu co
      if (id != null)
      {
        parameters.Add("Id", id);
      }
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      //var sqlQuery = "EXEC dbo.HS_Customer_Search @Param1";
      string sqlStore = "HS_Booking1_Payment_ups";
      var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

      //query store param 
      string paramfromstore = "select parameter_name,data_type from information_schema.parameters where specific_name = '" + sqlStore + "'";

      var paramfromstoreList = new List<dynamic>();
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(paramfromstore, connection))
        {
          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row.Add(reader.GetName(i), reader.GetValue(i));
              }
              paramfromstoreList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }

      //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
      // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
      var sqlParams = new List<SqlParameter>();
      for (int i = 0; i < paramfromstoreList.Count(); i++)
      {
        //param key
        string store_key = ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Substring(1, ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Length - 1);
        //param type
        string store_datatype = ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Substring(0, ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Length);
        //neu form field co chua param trong store thi xu ly tiep
        if (parameters.ContainsKey(store_key))
        {
          if (parameters[store_key] != null && parameters[store_key].ToString() != "")
          {
            object value;
            var check_date = new List<string> { "date", "datetime", "time" };
            var check_num = new List<string> { "int", "bigint", "decimal", "float" };
            //xu ly du lieu truoc khi dua vao store
            if (check_date.Contains(store_datatype))
            {
              DateTime date = Convert.ToDateTime(parameters[store_key]);
              string format = "yyyy-MM-dd HH:mm:ss";
              value = date.ToString(format);
            }
            else if (check_num.Contains(store_datatype))
            {
              double num = Convert.ToDouble(parameters[store_key]);
              value = num;
            }
            //else if (store_datatype == "TagBox" || store_datatype == "DropDownBox")
            //{
            //  var list = JArray.Parse(parameters[store_key].ToString());
            //  string res = list.Count == 0 ? null : string.Join(",", list);
            //  value = res;
            //}
            else
            {
              value = parameters[store_key].ToString();
            }

            sqlQuery.Append($" @{store_key} = @{store_key},");
            sqlParams.Add(new SqlParameter($"@{store_key}", value ?? DBNull.Value));
          }
        }
      }

      //foreach (var param in parameters)
      //{
      //  sqlQuery.Append($" @{param.Key} = @{param.Key},");
      //  sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value ?? DBNull.Value));
      //}

      // Xóa dấu phẩy cuối cùng
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }

      var resultList = new List<dynamic>();
      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(sqlParams.ToArray());

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row.Add(reader.GetName(i), reader.GetValue(i));
              }
              resultList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }

      return resultList;
    }

    public async Task<IDictionary<string, object>> HS_Booking1_Payment_sel(int? Id)
    {
      var connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
      string sqlStore = "HS_Booking1_Payment_sel";
      var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

      // Dictionary chứa các tham số
      var parameters = new Dictionary<string, object>
      {
          { "Id", Id}
      };

      var sqlParams = new List<SqlParameter>();
      foreach (var param in parameters)
      {
        sqlQuery.Append($" @{param.Key} = @{param.Key},");
        sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value));
      }

      // Xóa dấu phẩy cuối cùng
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }

      var resultList = new List<dynamic>();

      using (var connection = new SqlConnection(connectionString))
      {
        if (connection.State != ConnectionState.Open)
        {
          await connection.OpenAsync();
        }
        using (var command = new SqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(sqlParams.ToArray());

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                string value = reader.GetValue(i).ToString();
                string key = reader.GetName(i).ToString();
                // Nếu giá trị là chuỗi rỗng, gán `null`
                if (string.IsNullOrEmpty(value))
                {
                  value = null;
                }
                row.Add(key, value);
              }
              resultList.Add(row);
            }
          }
        }
      }
      // Chuyển đổi List<dynamic> thành Dictionary<string, object>
      var dictionary = resultList
    .SelectMany(obj => ((IDictionary<string, object>)obj)
        .Select(prop => new KeyValuePair<string, object>(prop.Key, prop.Value)))
    .ToDictionary(pair => pair.Key, pair => pair.Value);

      // nhan du lieu duoi dang object
      return dictionary;
    }

  }
}
