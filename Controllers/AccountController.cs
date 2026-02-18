using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using KOAHome.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using KOAHome.EntityFramework;
using KOAHome.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace KOAHome.Controllers
{
  [AllowAnonymous]
  public class AccountController : Controller
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    private readonly IAttachmentService _att;

    private readonly TttConfigContext _dbconfig;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IAttachmentService att,
        TttConfigContext dbconfig)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
      _att = att;
      _dbconfig = dbconfig;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
      ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/Dashboards/KoaDashboard"); // nếu không có thì về "/"
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
    {
      if (!ModelState.IsValid)
        return View(model);

      var user = await _userManager.FindByNameAsync(model.Username);
      if (user != null)
      {
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          var listRoleName = string.Join(", ",
              user.Roles
                  .Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(code => code.Trim())
                  .Join(
                      _roleManager.Roles, // Explicitly specify the type arguments  
                      code => code,
                      role => role.Code,
                      (code, role) => role.DisplayName
                  )
          );

          // lấy thông tin tenant từ NetTenants
          var tenantInfo = _dbconfig.Users
              .Where(u => u.Id == user.Id)
              .Join(_dbconfig.NetTenants,
                    u => u.SiteId,
                    t => t.Id,
                    (u, t) => new
                    {
                      TenantCode = t.Code,
                      TenantName = t.Name,
                      TenantShortName = t.Shortname,
                      TenantDescription = t.Description,
                      TenantLogoUrl = t.Tenantlogourl,
                      TenantIcoUrl = t.Tenanticourl,
                      TenantLogoTextUrl = t.Tenantlogotexturl
                    })
              .FirstOrDefault();

          // thêm các thông tin cơ bản vào cookie
          var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
          identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
          identity.AddClaim(new Claim("UserID", user.Id.ToString()));
          identity.AddClaim(new Claim("FullName", user.FullName));
          identity.AddClaim(new Claim("AvatarImgUrl", user.AvatarImgUrl));
          identity.AddClaim(new Claim("Roles", listRoleName ?? ""));
          identity.AddClaim(new Claim("SiteId", user.SiteId.ToString() ?? ""));
          identity.AddClaim(new Claim("SiteCode", tenantInfo.TenantCode ?? ""));
          identity.AddClaim(new Claim("SiteName", tenantInfo.TenantName ?? ""));
          identity.AddClaim(new Claim("SiteShortName", tenantInfo.TenantShortName ?? ""));
          identity.AddClaim(new Claim("TenantLogoUrl", tenantInfo.TenantLogoUrl ?? ""));
          identity.AddClaim(new Claim("TenantIcoUrl", tenantInfo.TenantIcoUrl ?? ""));
          identity.AddClaim(new Claim("TenantLogoTextUrl", tenantInfo.TenantLogoTextUrl ?? ""));

          var principal = new ClaimsPrincipal(identity);
          await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme, // "Identity.Application"
                principal);
          return RedirectToLocal(returnUrl);
        }
      }

      ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
      return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
      var model = new RegisterModel
      {
        AvailableRoles = _roleManager.Roles.Select(r => new SelectListItem
        {
          Value = r.Name,
          Text = r.DisplayName ?? r.Name
        }).ToList(),
        AvailableSites = _dbconfig.NetTenants.Select(s => new SelectListItem
        {
          Value = s.Id.ToString(),
          Text = s.Name
        }).ToList()
      };

      // xu ly file
      // Kiểm tra xem form có file nào không
      // lay danh sach object type code tu config form neu co field file uploader
      ViewData["fileUrls"] = await _att.HandleFiles("NET_AspNetUser_Avatar", null, 0);

      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model, [FromForm] IFormCollection form)
    {
      if (!ModelState.IsValid)
      {
        var roles = _roleManager.Roles.ToList();   // ép EF query trước
        model.AvailableRoles = roles.Select(r => new SelectListItem
        {
          Value = r.Name,
          Text = r.DisplayName ?? r.Name
        }).ToList();

        var sites = _dbconfig.NetTenants.ToList(); // ép EF query trước
        model.AvailableSites = sites.Select(s => new SelectListItem
        {
          Value = s.Id.ToString(),
          Text = s.Name
        }).ToList();
        return View(model);
      }

      // ✅ Kiểm tra AdminPassword
      var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
      bool isValidAdminPassword = false;

      foreach (var admin in adminUsers)
      {
        // Dùng CheckPasswordAsync để so sánh password hash
        if (await _userManager.CheckPasswordAsync(admin, model.AdminPassword))
        {
          isValidAdminPassword = true;
          break;
        }
      }

      if (!isValidAdminPassword)
      {
        ModelState.AddModelError("AdminPassword", "Mật khẩu Admin không hợp lệ.");
        var roles = _roleManager.Roles.ToList();   // ép EF query trước
        model.AvailableRoles = roles.Select(r => new SelectListItem
        {
          Value = r.Name,
          Text = r.DisplayName ?? r.Name
        }).ToList();

        var sites = _dbconfig.NetTenants.ToList(); // ép EF query trước
        model.AvailableSites = sites.Select(s => new SelectListItem
        {
          Value = s.Id.ToString(),
          Text = s.Name
        }).ToList();
        return View(model);
      }

      var user = new ApplicationUser
      {
        UserName = model.Username,
        Email = model.Email,
        FullName = model.FullName,
        PhoneNumber = model.PhoneNumber,
        AvatarImgUrl = model.AvatarImgUrl
                                          ,
        SiteId = model.SiteId,
        SiteName = model.SiteName,
        Position = model.Position,
        Roles = model.SelectedRoles[0].ToString()
      };
      var result = await _userManager.CreateAsync(user, model.Password);
      if (result.Succeeded)
      {
        // ✅ Gán nhiều role
        if (model.SelectedRoles != null && model.SelectedRoles.Any())
        {
          var roles = model.SelectedRoles[0].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(r => r.Trim())
                      .ToList();

          await _userManager.AddToRolesAsync(user, roles);
        }
        //// Gán role mặc định
        //if (!await _roleManager.RoleExistsAsync("Admin")){
        //var role = new ApplicationRole
        //{
        //  Name = "Admin",
        //  Code = "Admin",
        //  DisplayName = "Quản trị viên",
        //  PageRedirect = "/Dashboards/KoaDashboard",
        //  DefaultMenuId = 3045,
        //  SiteId = 1,
        //  SiteCode = "NET",
        //  Description = "Quản trị viên",
        //  CreationTime = DateTime.UtcNow
        //};
        //await _roleManager.CreateAsync(role);
        //await _userManager.AddToRolesAsync(user, new[] { "Admin" });
        //}

        // xu ly file
        // Kiểm tra xem form có file nào không
        // lay danh sach object type code tu config form neu co field file uploader
        var attResult = await _att.HandleFiles("NET_AspNetUser_Avatar", form, user.Id);

        // ✅ Lưu AvatarImgUrl vào AspNetUsers
        if (attResult.TryGetValue("NET_AspNetUser_Avatar", out var urls) && urls.Any())
        {
          user.AvatarImgUrl = urls.First();
          await _userManager.UpdateAsync(user);
        }
        // xu ly luu bang attachment
        var saveAttachmentResult = await _att.SaveAttachmentTable(form, user.Id);

        // Dùng JsonConvert để chuyển về JObject hoặc dynamic
        var json = JObject.FromObject(saveAttachmentResult); // nếu dùng Newtonsoft.Json
        bool success = json["success"]?.Value<bool>() ?? false;

        if (!success)
        {
          string error = json["errorMessage"]?.ToString();
          TempData["ErrorMessage"] = error ?? "Lưu file không thành công";
          ModelState.AddModelError(string.Empty, error ?? "Lưu file không thành công");
          return View(model);
        }

        return RedirectToAction("Login", "Account");
      }

      foreach (var error in result.Errors)
      {

        string message = error.Code switch
        {
          "DuplicateUserName" => "Tên đăng nhập đã tồn tại.",
          "DuplicateEmail" => "Email đã được sử dụng.",
          "PasswordTooShort" => "Mật khẩu quá ngắn. Vui lòng chọn mật khẩu dài hơn.",
          "PasswordRequiresNonAlphanumeric" => "Mật khẩu cần ít nhất một ký tự đặc biệt.",
          "PasswordRequiresDigit" => "Mật khẩu cần ít nhất một số.",
          "PasswordRequiresUpper" => "Mật khẩu cần ít nhất một chữ hoa.",
          "PasswordRequiresLower" => "Mật khẩu cần ít nhất một chữ thường.",
          _ => error.Description // fallback nếu không khớp code
        };
        ModelState.AddModelError(string.Empty, message);
      }

      model.AvailableRoles = _roleManager.Roles.Select(r => new SelectListItem
      {
        Value = r.Name,
        Text = r.DisplayName ?? r.Name
      }).ToList();
      model.AvailableSites = _dbconfig.NetTenants.Select(s => new SelectListItem
      {
        Value = s.Id.ToString(),
        Text = s.Name
      }).ToList();
      return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
      // xóa session menu hiện tại
      HttpContext.Session.Remove("CurrentMenuCode");

      await _signInManager.SignOutAsync();
      return RedirectToAction("Login", "Account");
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
      if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        return Redirect(returnUrl);

      return RedirectToAction("KoaDashboard", "Dashboards");
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
    {
      var user = await _userManager.GetUserAsync(User);
      bool hasGoogle = false;

      //nếu đang đăng nhập vào google thì đăng xuất
      if (user != null)
      {
        var logins = await _userManager.GetLoginsAsync(user);
        hasGoogle = logins.Any(l => l.LoginProvider == "Google");
      }

      // Nếu user đã đăng nhập bằng Google → redirect Google logout
      if (hasGoogle)
      {
        string googleLogoutUrl = "https://accounts.google.com/Logout";
        return Redirect(googleLogoutUrl);
      }
      var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
      var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
      return Challenge(properties, provider);
    }


    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
    {
      if (remoteError != null)
      {
        ModelState.AddModelError(string.Empty, $"Lỗi từ provider: {remoteError}");
        return RedirectToAction("Login");
      }

      var info = await _signInManager.GetExternalLoginInfoAsync();
      if (info == null)
      {
        return RedirectToAction("Login");
      }

      // ✅ Bước 1: Đăng nhập nếu user đã có login
      var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
      if (signInResult.Succeeded)
      {
        return RedirectToLocal(returnUrl);
      }

      // ✅ Bước 2: Nếu chưa có user → tạo user mới
      var email = info.Principal.FindFirstValue(ClaimTypes.Email);
      if (email != null)
      {
        var user = new ApplicationUser
        {
          UserName = email,
          Email = email,
          FullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
          // ✅ Liên kết user với Google → tự động insert vào AspNetUserLogins
          await _userManager.AddLoginAsync(user, info);

          // Đăng nhập user
          await _signInManager.SignInAsync(user, isPersistent: false);

          //var picture = info.Principal.FindFirstValue("picture");
          //await _userManager.AddClaimAsync(user, new Claim("AvatarUrl", picture));

          return RedirectToLocal(returnUrl);
        }
      }

      return RedirectToAction("Login");
    }
  }
}
