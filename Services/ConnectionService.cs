
using Microsoft.EntityFrameworkCore;
using KOAHome.EntityFramework;
using Microsoft.Data.SqlClient;
using System.Dynamic;
using System.Text;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace KOAHome.Services
{
  public interface IConnectionService
  {
    public Task<(StringBuilder SqlQuery, List<NpgsqlParameter> SqlParam)> Connection_GetQueryParam_Simple(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<(StringBuilder SqlQuery, List<NpgsqlParameter> SqlParam)> Connection_GetQueryParam(Dictionary<string, object> parameters, string sqlStore, string? connectionString);
    public Task<List<dynamic>> Connection_GetDataFromQuery(Dictionary<string, object> parameters, string sqlStore, string? connectionString, StringBuilder sqlQuery, List<NpgsqlParameter> sqlParams);
    public Task<IDictionary<string, object>?> Connection_GetSingleDataFromQuery(Dictionary<string, object> parameters, string sqlStore, string? connectionString, StringBuilder sqlQuery, List<NpgsqlParameter> sqlParams);
    public bool CheckForErrors(List<dynamic> resultList, out string errorMessage);

  }
  public class ConnectionService : IConnectionService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    public ConnectionService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
    }


    public async Task<(StringBuilder SqlQuery, List<NpgsqlParameter> SqlParam)> Connection_GetQueryParam_Simple(
       Dictionary<string, object> parameters,
       string sqlStore,
       string? connectionString)
    {
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection");
      }

      var sqlQuery = new StringBuilder("CALL dbo." + sqlStore + "(");

      // Xây dựng tham số động
      var sqlParams = new List<NpgsqlParameter>();
      foreach (var param in parameters)
      {
        string paramName = param.Key;
        string cleanParamName = paramName.TrimStart('_'); // PostgreSQL dùng "_" cho tham số IN
        if (parameters.ContainsKey(cleanParamName))
        {
          var value = parameters[cleanParamName];
          sqlQuery.Append($"@{cleanParamName} => @{cleanParamName},");
          sqlParams.Add(new NpgsqlParameter($"@{cleanParamName}", value ?? DBNull.Value));
        }
      }

      // Xóa dấu phẩy cuối và đóng ngoặc
      if (sqlParams.Count > 0)
      {
        sqlQuery.Length--;
      }
      sqlQuery.Append(");");

      return (sqlQuery, sqlParams);
    }

    public async Task<(StringBuilder SqlQuery, List<NpgsqlParameter> SqlParam)> Connection_GetQueryParam(
           Dictionary<string, object> parameters,
           string sqlStore,
           string? connectionString)
        {
            //lower ten store
            sqlStore = sqlStore.ToLower();

            if (connectionString == null)
            {
                connectionString = _configuration.GetConnectionString("DefaultConnection");
            }

            var sqlQuery = new StringBuilder("CALL dbo." + sqlStore + "(");

            // Lấy danh sách tham số của procedure từ PostgreSQL
            string paramQuery = @"
               SELECT 
                   pg_get_function_arguments(p.oid) AS parameters
               FROM 
                   pg_proc p
               WHERE 
                   p.proname = @procName;
           ";

            var paramList = new List<string>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(paramQuery, connection))
                {
                    command.Parameters.AddWithValue("@procName", sqlStore);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var paramString = reader.GetString(0);
                            if (paramString != null && paramString != "")
                            {
                              paramList = paramString.Split(',')
                                  .Select(p => p.Trim().Split(' ')[1].Replace("_", "")) // Ví dụ: "_reportid integer" -> "_reportid"
                                  .ToList();
                            }
                        }
                    }
                }
            }

            // Xây dựng tham số động
            var sqlParams = new List<NpgsqlParameter>();
            foreach (var paramName in paramList)
            {
                string cleanParamName = paramName.TrimStart('_'); // PostgreSQL dùng "_" cho tham số IN
                if (parameters.ContainsKey(cleanParamName))
                {
                    var value = parameters[cleanParamName];
                    sqlQuery.Append($"_{cleanParamName} := @{cleanParamName},");
                    sqlParams.Add(new NpgsqlParameter($"@{cleanParamName}", value ?? DBNull.Value));
                }
            }

            // Xóa dấu phẩy cuối và đóng ngoặc
            if (sqlParams.Count > 0)
            {
                sqlQuery.Length--;
            }
            sqlQuery.Append(");");

            return (sqlQuery, sqlParams);
        }

    public async Task<List<dynamic>> Connection_GetDataFromQuery(
        Dictionary<string, object> parameters,
        string sqlStore,
        string? connectionString,
        StringBuilder sqlQuery,
        List<NpgsqlParameter> SqlParams)
    {
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection");
      }

      var resultList = new List<dynamic>();
      using (var connection = new NpgsqlConnection(connectionString))
      {
        await connection.OpenAsync();

        // Gọi procedure để tạo bảng tạm
        using (var command = new NpgsqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(SqlParams.ToArray());
          await command.ExecuteNonQueryAsync();
        }

        // Đọc dữ liệu từ bảng tạm tmp_result
        using (var selectCommand = new NpgsqlCommand("SELECT * FROM tmp_result;", connection))
        {
          using (var reader = await selectCommand.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                object? value = null;
                // xu ly gia tri null hoac {} truyen vao gay loi
                if (reader.IsDBNull(i))
                {
                  value = null;
                }
                else
                {
                  value = (object)reader.GetValue(i);
                }
                string key = reader.GetName(i).ToString();

                row.Add(key, value);
              }
              resultList.Add(row);
            }
          }
        }
        await connection.CloseAsync();
      }
      return resultList;
    }

    public async Task<IDictionary<string, object>?> Connection_GetSingleDataFromQuery(
        Dictionary<string, object> parameters,
        string sqlStore,
        string? connectionString,
        StringBuilder sqlQuery,
        List<NpgsqlParameter> SqlParams)
    {
      if (connectionString == null)
      {
        connectionString = _configuration.GetConnectionString("DefaultConnection");
      }

      var resultList = new List<dynamic>();
      using (var connection = new NpgsqlConnection(connectionString))
      {
        await connection.OpenAsync();

        // Gọi procedure để tạo bảng tạm
        using (var command = new NpgsqlCommand(sqlQuery.ToString(), connection))
        {
          command.Parameters.AddRange(SqlParams.ToArray());
          await command.ExecuteNonQueryAsync();
        }

        // Đọc dữ liệu từ bảng tạm tmp_result
        using (var selectCommand = new NpgsqlCommand("SELECT * FROM tmp_result;", connection))
        {
          using (var reader = await selectCommand.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var row = new ExpandoObject() as IDictionary<string, object>;
              for (int i = 0; i < reader.FieldCount; i++)
              {
                object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                string key = reader.GetName(i);
                row[key] = value;
              }

              return row; // ✅ chỉ trả về dòng đầu tiên
            }
          }
        }
        await connection.CloseAsync();
      }

      return null;
    }

    //public async Task<(StringBuilder SqlQuery, List<SqlParameter> SqlParam)> Connection_GetQueryParam_Simple(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
    //{
    //  // neu khong truyen connect string thi se lay connection string mac dinh
    //  if (connectionString == null)
    //  {
    //    connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
    //  }
    //  var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

    //  //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
    //  // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
    //  var sqlParams = new List<SqlParameter>();
    //  foreach (var param in parameters)
    //  {
    //    sqlQuery.Append($" @{param.Key} = @{param.Key},");
    //    sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value ?? (object)DBNull.Value));
    //  }

    //  // Xóa dấu phẩy cuối cùng
    //  if (sqlParams.Count > 0)
    //  {
    //    sqlQuery.Length--;
    //  }

    //  return (sqlQuery, sqlParams);
    //}

    //public async Task<(StringBuilder SqlQuery, List<SqlParameter> SqlParam)> Connection_GetQueryParam(Dictionary<string, object> parameters, string sqlStore, string? connectionString)
    //{
    //  // neu khong truyen connect string thi se lay connection string mac dinh
    //  if (connectionString == null)
    //  {
    //    connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
    //  }
    //  var sqlQuery = new StringBuilder("EXEC dbo." + sqlStore);

    //  //query store param 
    //  string paramfromstore = "select parameter_name,data_type from information_schema.parameters where specific_name = '" + sqlStore + "'";

    //  var paramfromstoreList = new List<dynamic>();
    //  using (var connection = new SqlConnection(connectionString))
    //  {
    //    if (connection.State != ConnectionState.Open)
    //    {
    //      await connection.OpenAsync();
    //    }
    //    using (var command = new SqlCommand(paramfromstore, connection))
    //    {
    //      using (var reader = await command.ExecuteReaderAsync())
    //      {
    //        while (await reader.ReadAsync())
    //        {
    //          var row = new ExpandoObject() as IDictionary<string, object>;
    //          for (int i = 0; i < reader.FieldCount; i++)
    //          {
    //            row.Add(reader.GetName(i), reader.GetValue(i));
    //          }
    //          paramfromstoreList.Add(row);
    //        }
    //      }
    //    }
    //    await connection.CloseAsync();
    //  }

    //  //voi du lieu param lay tu store, kiem tra dinh dang de xu ly
    //  // kiem tra du lieu nhap vao co trong store thi dua vao lenh xu ly
    //  var sqlParams = new List<SqlParameter>();
    //  for (int i = 0; i < paramfromstoreList.Count(); i++)
    //  {
    //    //param key
    //    string store_key = ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Substring(1, ((IDictionary<string, object>)paramfromstoreList[i])["parameter_name"].ToString().Length - 1);
    //    //param type
    //    string store_datatype = ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Substring(0, ((IDictionary<string, object>)paramfromstoreList[i])["data_type"].ToString().Length);
    //    //neu form field co chua param trong store thi xu ly tiep
    //    if (parameters.ContainsKey(store_key))
    //    {
    //      if (parameters[store_key] != null && parameters[store_key].ToString() != "")
    //      {
    //        object value;
    //        var check_date = new List<string> { "date", "datetime", "time" };
    //        var check_num = new List<string> { "int", "bigint", "decimal", "float" };
    //        //xu ly du lieu truoc khi dua vao store
    //        if (check_date.Contains(store_datatype))
    //        {
    //          DateTime date = Convert.ToDateTime(parameters[store_key]);
    //          string format = "yyyy-MM-dd HH:mm:ss";
    //          value = date.ToString(format);
    //        }
    //        else if (check_num.Contains(store_datatype))
    //        {
    //          double num = Convert.ToDouble(parameters[store_key]);
    //          value = num;
    //        }
    //        //else if (store_datatype == "TagBox" || store_datatype == "DropDownBox")
    //        //{
    //        //  var list = JArray.Parse(parameters[store_key].ToString());
    //        //  string res = list.Count == 0 ? null : string.Join(",", list);
    //        //  value = res;
    //        //}
    //        else
    //        {
    //          value = parameters[store_key].ToString();
    //        }

    //        sqlQuery.Append($" @{store_key} = @{store_key},");
    //        sqlParams.Add(new SqlParameter($"@{store_key}", value ?? DBNull.Value));
    //      }
    //    }
    //  }

    //  //foreach (var param in parameters)
    //  //{
    //  //  sqlQuery.Append($" @{param.Key} = @{param.Key},");
    //  //  sqlParams.Add(new SqlParameter($"@{param.Key}", param.Value ?? DBNull.Value));
    //  //}

    //  // Xóa dấu phẩy cuối cùng
    //  if (sqlParams.Count > 0)
    //  {
    //    sqlQuery.Length--;
    //  }

    //  return (sqlQuery, sqlParams);
    //}

    //public async Task<List<dynamic>> Connection_GetDataFromQuery(Dictionary<string, object> parameters, string sqlStore, string? connectionString, StringBuilder sqlQuery, List<SqlParameter> sqlParams)
    //{
    //  // neu khong truyen connect string thi se lay connection string mac dinh
    //  if (connectionString == null)
    //  {
    //    connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
    //  }

    //  var resultList = new List<dynamic>();
    //  using (var connection = new SqlConnection(connectionString))
    //  {
    //    if (connection.State != ConnectionState.Open)
    //    {
    //      await connection.OpenAsync();
    //    }
    //    using (var command = new SqlCommand(sqlQuery.ToString(), connection))
    //    {
    //      command.Parameters.AddRange(sqlParams.ToArray());

    //      using (var reader = await command.ExecuteReaderAsync())
    //      {
    //        while (await reader.ReadAsync())
    //        {
    //          var row = new ExpandoObject() as IDictionary<string, object>;
    //          for (int i = 0; i < reader.FieldCount; i++)
    //          {
    //            object? value = null;
    //            // xu ly gia tri null hoac {} truyen vao gay loi
    //            if (reader.IsDBNull(i))
    //            {
    //              value = null;
    //            }
    //            else
    //            {
    //              value = (object)reader.GetValue(i);
    //            }
    //            string key = reader.GetName(i).ToString();

    //            row.Add(key, value);
    //          }
    //          resultList.Add(row);
    //        }
    //      }
    //    }
    //    await connection.CloseAsync();
    //  }

    //  return resultList;
    //}

    //public async Task<IDictionary<string, object>?> Connection_GetSingleDataFromQuery(Dictionary<string, object> parameters, string sqlStore, string? connectionString, StringBuilder sqlQuery, List<SqlParameter> sqlParams)
    //{
    //  // neu khong truyen connect string thi se lay connection string mac dinh
    //  if (connectionString == null)
    //  {
    //    connectionString = _configuration.GetConnectionString("DefaultConnection"); // Thay thế bằng chuỗi kết nối của bạn
    //  }

    //  using (var connection = new SqlConnection(connectionString))
    //  {
    //    if (connection.State != ConnectionState.Open)
    //    {
    //      await connection.OpenAsync();
    //    }
    //    using (var command = new SqlCommand(sqlQuery.ToString(), connection))
    //    {
    //      command.Parameters.AddRange(sqlParams.ToArray());

    //      using (var reader = await command.ExecuteReaderAsync())
    //      {
    //        if (await reader.ReadAsync())
    //        {
    //          var row = new ExpandoObject() as IDictionary<string, object>;
    //          for (int i = 0; i < reader.FieldCount; i++)
    //          {
    //            object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
    //            string key = reader.GetName(i);
    //            row[key] = value;
    //          }

    //          return row; // ✅ chỉ trả về dòng đầu tiên
    //        }
    //      }
    //    }
    //    await connection.CloseAsync();
    //  }

    //  return null;
    //}

    // kiểm tra lỗi và trả về message
    public bool CheckForErrors(List<dynamic> resultList, out string errorMessage)
    {
      var errorMessages = resultList
          .Where(item => ((IDictionary<string, object>)item).ContainsKey("ErrorMessage"))
          .Select(item => item.ErrorMessage.ToString().Replace("\\n", "<br/>"))
          .ToList();

      if (errorMessages.Any())
      {
        errorMessage = string.Join(", ", errorMessages);
        return true;
      }

      errorMessage = null;
      return false;
    }
  }
}
