using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    public class CollectorJob: IJob {
        private readonly ILogger<CollectorJob> _logger;
        public CollectorJob(ILogger<CollectorJob> logger) {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context) {
            Console.WriteLine($"Collector execute");
            _logger.LogInformation($"{DateTime.Now} Collector:");

            return Task.CompletedTask;
        }
    }
}
