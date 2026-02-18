using KOAHome.EntityFramework;
using KOAHome.Models;
using KOAHome.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using System.ClientModel;
using System.Data.Common;
using Amazon.S3;
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
              Configuration.GetConnectionString("DefaultConnection"))
      //options.UseSqlServer(sqlOptions =>
      //{
      //  Configuration.GetConnectionString("DefaultConnection");
      //  sqlOptions.CommandTimeout(300); // Thiết lập CommandTimeout là 300 giây

      //})
      );
      services.AddDbContext<TttConfigContext>(options =>
          options.UseNpgsql(
              Configuration.GetConnectionString("ConfigConnection"))
          //options.UseSqlServer(sqlOptions =>
          //{
          //  Configuration.GetConnectionString("ConfigConnection");
          //  sqlOptions.CommandTimeout(300); // Thiết lập CommandTimeout là 300 giây
          //})
        );
      services.AddDistributedMemoryCache();
      services.AddResponseCaching();
      services.AddSession(options => {
        options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
      });

      // Cấu hình Identity - Cookie based authentication
      services.AddIdentity<ApplicationUser, ApplicationRole>()
          .AddEntityFrameworkStores<TttConfigContext>()
          .AddDefaultTokenProviders();

      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // tùy chọn RememberMe sẽ ghi đè
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
      });

      var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
      var googleAuthClientId = "";
      var googleAuthClientSecret = "";
      if (env == "Development")
      {
        googleAuthClientId = Configuration["Authentication:Google:ClientId"];
        googleAuthClientSecret = Configuration["Authentication:Google:ClientSecret"];
      }
      else
      {
        googleAuthClientId = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_ID");
        googleAuthClientSecret = Environment.GetEnvironmentVariable("GOOGLE_AUTH_CLIENT_SECRET");
      }
      services.AddAuthentication()
      .AddGoogle(options =>
      {
        options.ClientId = googleAuthClientId;
        options.ClientSecret = googleAuthClientSecret;
      });


      services.AddDataProtection()
          .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
          .SetApplicationName("KOAHome");

      services.Configure<ForwardedHeadersOptions>(options => {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
      });

      services.AddHttpClient();
      services.AddHttpContextAccessor();
      services.AddControllersWithViews(options =>
      {
        // Áp dụng AuthorizeAttribute cho toàn bộ controller/action
        var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
      })
          .AddViewOptions(options => options.HtmlHelperOptions.ClientValidationEnabled = false);

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
      services.AddScoped<IGoogleSheetService, GoogleSheetService>();
      services.AddScoped<GeminiService>();
      services.AddScoped<DeepSeekService>();
      services.AddTransient<Func<string, IAiService>>(serviceProvider => key =>
      {
        return key.ToLower() switch
        {
          // Ép kiểu tường minh về IAiService
          "gemini" => (IAiService)serviceProvider.GetRequiredService<GeminiService>(),
          "deepseek" => (IAiService)serviceProvider.GetRequiredService<DeepSeekService>(),
          _ => throw new KeyNotFoundException("Không tìm thấy dịch vụ AI tương ứng")
        };
      });
      services.Configure<CloudflareR2Config>(Configuration.GetSection("CloudflareR2"));
      services.Configure<FormOptions>(options =>
      {
        options.MultipartBodyLengthLimit = 104857600; // 100MB
      });

      services.AddSingleton(sp =>
      {
        var config = sp.GetRequiredService<IConfiguration>();

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var apiKey = "";
        if (env == "Development")
        {
          apiKey = config["OpenRouter:ApiKey"];
        }
        else
        {
          apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");
        }

        return new OpenAIClient(
            new ApiKeyCredential(apiKey),
            new OpenAIClientOptions
            {
              Endpoint = new Uri(config["OpenRouter:BaseUrl"])
            }
        );
      });

      // add health check for deploy
      services.AddHealthChecks();
          //.AddNpgSql(Configuration.GetConnectionString("DefaultConnection"));
      services.Configure<CloudflareR2Config>(Configuration.GetSection("CloudflareR2"));
      services.Configure<FormOptions>(options =>
      {
        options.MultipartBodyLengthLimit = 104857600; // 100MB
      });
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

      app.UseHealthChecks("/health");
      app.UseAuthentication(); // ⚠️ Phải có
      app.UseAuthorization();

      app.UseSession();

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHealthChecks("/health").AllowAnonymous();
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
