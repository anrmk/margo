using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    public class NotifyJob: IJob {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotifyJob> _logger;

        public NotifyJob(ILogger<NotifyJob> logger, IServiceProvider serviceProvider) {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context) {
            //var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
            //var invoices = _context.Invoices.ToList();


            //var invoices = await invoiceManager.All();

            Console.WriteLine($"Job execute");
            _logger.LogInformation($"{DateTime.Now} Invoice count:");

            return Task.CompletedTask;
        }
    }
}
