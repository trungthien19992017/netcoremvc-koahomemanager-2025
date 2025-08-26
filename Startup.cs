using KOAHome.EntityFramework;
using Microsoft.EntityFrameworkCore;
using KOAHome.Services;
using System.Data.Common;
using Amazon.S3;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.DataProtection;
using KOAHome.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
//using KOAHome.Services;

namespace KOAHome
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.


    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<QLKCL_NEWContext>(options =>
          options.UseNpgsql(
              sqlOptions =>
              {
                  Configuration.GetConnectionString("DefaultConnection");
                  sqlOptions.CommandTimeout(300); // Thiết lập CommandTimeout là 300 giây

              }));
      services.AddDbContext<TttConfigContext>(options =>
          options.UseNpgsql(
              sqlOptions =>
              {
                  Configuration.GetConnectionString("ConfigConnection");
                  sqlOptions.CommandTimeout(300); // Thiết lập CommandTimeout là 300 giây
              }));
      services.AddDistributedMemoryCache();
      services.AddResponseCaching();
      services.AddSession(options => {
        options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
      });

        //// Cấu hình Identity - Cookie based authentication
        //services.AddIdentity<ApplicationUser, ApplicationRole>()
        //    .AddEntityFrameworkStores<TttConfigContext>()
        //    .AddDefaultTokenProviders();

        //services.ConfigureApplicationCookie(options =>
        //{
        //    options.LoginPath = "/Account/Login";
        //    options.AccessDeniedPath = "/Account/AccessDenied";
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // tùy chọn RememberMe sẽ ghi đè
        //    options.SlidingExpiration = true;
        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //    options.Cookie.SameSite = SameSiteMode.Strict;
        //});

        //services.AddAuthentication()
        //.AddGoogle(options =>
        //{
        //    options.ClientId = Configuration["Authentication:Google:ClientId"];
        //    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
        //});

        services.AddDataProtection()
          .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
          .SetApplicationName("KOAHome");

      services.Configure<ForwardedHeadersOptions>(options => {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
      });

      services.AddHttpContextAccessor();
      services.AddControllersWithViews();
      services.AddScoped<IReportEditorService, ReportEditorService>();
      services.AddScoped<IAttachmentService, AttachmentService>();
      services.AddScoped<IReportService, ReportService>();
      services.AddScoped<IFormService, FormService>();
      services.AddScoped<IActionService, ActionService>();
      services.AddScoped<IConnectionService, ConnectionService>();
      services.AddScoped<IWidgetService, WidgetService>();
      services.AddScoped<IDRDatasourceService, DRDatasourceService>();
      services.AddScoped<INetServiceService, NetServiceService>();
      services.AddScoped<INetMenuService, NetMenuService>();
      services.AddScoped<INetTabPanelService, NetTabPanelService>();
      services.AddScoped<INetFormWizardService, NetFormWizardService>();
      services.Configure<CloudflareR2Config>(Configuration.GetSection("CloudflareR2"));
      services.Configure<FormOptions>(options =>
      {
        options.MultipartBodyLengthLimit = 104857600; // 100MB
      });
      //services.AddSingleton(s =>
      //{
      //  var config = Configuration.GetSection("CloudflareR2").Get<CloudflareR2Config>();

      //  var s3Config = new AmazonS3Config
      //  {
      //    RegionEndpoint = Amazon.RegionEndpoint.USEast1, // Không ảnh hưởng vì R2 là regionless
      //    ServiceURL = $"https://{config.AccountId}.r2.cloudflarestorage.com",
      //    ForcePathStyle = true
      //  };

      //  return new AmazonS3Client(config.AccessKey, config.SecretKey, s3Config);
      //});

      // add health check for deploy
      services.AddHealthChecks()
          .AddNpgSql(Configuration.GetConnectionString("DefaultConnection"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Pages/MiscError");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      if (env.IsDevelopment())
      {
        app.UseHttpsRedirection();
      }
      app.UseStaticFiles();

      app.UseRouting();

      app.UseResponseCaching();

        app.UseAuthentication(); // ⚠️ Phải có
      app.UseAuthorization();

      app.UseSession();

      app.UseHealthChecks("/health");

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Dashboards}/{action=KoaDashboard}/{id?}");
        // route cho report
        endpoints.MapControllerRoute(
              name: "report/viewer-utility",
              pattern: "report/viewer-utility/{reportCode}",
              defaults: new { controller = "NETReport", action = "Viewer_Utility" }
          );
        // route cho editor
        endpoints.MapControllerRoute(
              name: "report/editor-utility",
              pattern: "report/editor-utility/{reportCode}/{id?}",
              defaults: new { controller = "NETReport", action = "Editor_Utility" }
          );
        // route cho form
        endpoints.MapControllerRoute(
              name: "form/viewer",
              pattern: "form/viewer/{formCode}/{id?}",
              defaults: new { controller = "NETForm", action = "Viewer"}
          );
        // route cho form popup
        endpoints.MapControllerRoute(
              name: "form/popup-viewer",
              pattern: "form/popup-viewer/{formCode}/{id?}",
              defaults: new { controller = "NETForm", action = "PopupForm" }
          );
        // route cho tab panel
        endpoints.MapControllerRoute(
              name: "tab/viewer",
              pattern: "tab/viewer/{tabCode}/{tabIndex}",
              defaults: new { controller = "NETTabPanel", action = "Viewer" }
          );
        // route cho form wizard
        endpoints.MapControllerRoute(
              name: "formwizard/viewer",
              pattern: "formwizard/viewer/{stepperCode}",
              defaults: new { controller = "NETFormWizard", action = "Viewer" }
          );
      });
    }
  }
}
