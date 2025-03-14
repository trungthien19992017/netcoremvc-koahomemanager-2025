
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
using System.Data;

namespace KOAHome.Services
{
  public interface IHsCustomerService
  {
    public Task<List<HsCustomer>> LoadListKhachHang();

  }
  public class HsCustomerService : IHsCustomerService
  {
    private readonly QLKCL_NEWContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration; 
    private const string CartSession = "CartSession";
    public HsCustomerService(QLKCL_NEWContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
      _db = db;
      _httpContextAccessor = httpContextAccessor;
      _configuration = configuration;
    }
    public async Task<List<HsCustomer>> LoadListKhachHang()
    {
      return await _db.HsCustomers.ToListAsync();
    }

  }
}
