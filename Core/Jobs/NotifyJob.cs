using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    public class NotifyJob: IJob {
        private readonly IServiceProvider _provider;
        private readonly ILogger<NotifyJob> _logger;

        public NotifyJob(IServiceProvider provider, ILogger<NotifyJob> logger) {
            _provider = provider;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context) {
            Console.WriteLine("Job execute");
            _logger.LogInformation($"{DateTime.Now} Hello world!");

            return Task.CompletedTask;
        }
    }
}
