using System;
using System.IO;

using Core.Context;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Web {
    public class Program {
        public static void Main(string[] args) {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;
                try {
                    ApplicationInitializer.Initialize(services);
                } catch(Exception ex) {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseStartup<Startup>();
                });
    }
}
