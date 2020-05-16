using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    public class NotifyJob: IJob {
        private readonly IServiceProvider _provider;
        private readonly ILogger<NotifyJob> _logger;

        public NotifyJob(ILogger<NotifyJob> logger, IServiceProvider provider) {
            _logger = logger;
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context) {
            using(var scope = _provider.CreateScope()) {
                var notification = scope.ServiceProvider.GetService<INotifyService>();

                var dbContext = scope.ServiceProvider.GetService<ApplicationContext>();
                var dbset = dbContext.Set<InvoiceEntity>();

                try {
                    var x = await dbset.ToListAsync();
                    var message = $"Collector execute {x.Count()} records at {DateTime.Now}.";
                    await notification.SendTextMessage(message);
                } catch(Exception e) {
                    _logger.LogInformation($"Notify Job: {e.Message}");
                }
            }
        }
    }
}
