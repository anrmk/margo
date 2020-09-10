using System;
using System.IO;

using Core.Context;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Web;

namespace Web {
    public class Program {
        public static void Main(string[] args) {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                try {
                    //ApplicationInitializer.Initialize(services);
                } catch(Exception exception) {
                    logger.Fatal(exception, "Program stopped because of an exception!");
                    throw;
                } finally {
                    NLog.LogManager.Shutdown();
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging => {
                    //logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .UseNLog();
    }
}
