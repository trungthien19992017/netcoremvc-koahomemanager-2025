using KOAHome.EntityFramework;
using Microsoft.EntityFrameworkCore;
using KOAHome.Services;
using System.Data.Common;
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
              Configuration.GetConnectionString("DefaultConnection")));
      services.AddDbContext<TttConfigContext>(options =>
          options.UseNpgsql(
              Configuration.GetConnectionString("ConfigConnection")));
      services.AddDistributedMemoryCache();
      services.AddSession(options => {
        options.IdleTimeout = TimeSpan.FromMinutes(20);//You can set Time
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
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
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseSession();

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
      });
    }
  }
}
