using KOAHome;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KOAHome
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();

              // Sử dụng /app/data-protection-keys thay vì /root
              var keysDir = Path.Combine(Directory.GetCurrentDirectory(), "data-protection-keys");
              Directory.CreateDirectory(keysDir);

              webBuilder.ConfigureServices(services => {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(keysDir))
                    .SetApplicationName("KOAHome");
              });

              webBuilder.UseUrls($"http://0.0.0.0:" + (Environment.GetEnvironmentVariable("PORT") ?? "8080"));
            });

  }
}
