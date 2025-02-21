// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Huxley2
{
   public static class Program
   {
       public static void Main(string[] args)
       {
           CreateHostBuilder(args).Build().Run();
       }

       public static IHostBuilder CreateHostBuilder(string[] args) =>
           // CreateDefaultBuilder adds default configuration sources and logging providers
           Host.CreateDefaultBuilder(args)
               // To enable debug logging, uncomment this ConfigureLogging block
               //.ConfigureLogging(logging =>
               //{
               //    logging.ClearProviders();
               //    logging.AddConsole();
               //    logging.AddDebug();
               //    logging.SetMinimumLevel(LogLevel.Debug);
               //})
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
                   webBuilder.UseKestrel(options =>
                   {
                       if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                       {
                           // Explicit port binding for Linux environments (e.g. Docker)
                           options.ListenAnyIP(80);
                       }

                       options.ConfigureEndpointDefaults(defaults =>
                       {
                           // To enable connection logging, uncomment this line
                           //defaults.UseConnectionLogging();

                           if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                           {
                               // HTTP/2 bug workaround for Windows 8.1/Server 2012 R2
                               defaults.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                           }
                       });
                   });
                   
                   if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                   {
                       // Windows-specific IIS configuration
                       webBuilder.UseIIS();
                       webBuilder.UseIISIntegration();
                   }
               });
   }
}
