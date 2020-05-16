using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities;
using Core.Extension;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    [DisallowConcurrentExecution]
    public class CollectorJob: IJob {
        private readonly ILogger<CollectorJob> _logger;
        private readonly IServiceProvider _provider;

        public CollectorJob(ILogger<CollectorJob> logger, IServiceProvider provider) {
            _logger = logger;
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context) {
            using(var scope = _provider.CreateScope()) {
                var dbContext = scope.ServiceProvider.GetService<ApplicationContext>();
                var notification = scope.ServiceProvider.GetService<INotifyService>();

                try {
                    var dbset = dbContext.Set<InvoiceEntity>();
                    var invoiceList = new List<InvoiceEntity>();

                    var companies = await dbContext.Set<CompanyEntity>().ToListAsync();
                    var vendors = await dbContext.Set<VendorEntity>().ToListAsync();

                    var rnd = new Random();
                    var count = rnd.Next(5, 15);
                    
                    for(var i = 0; i < count; i++) {
                        var company = companies[rnd.Next(0, companies.Count)];
                        var vendor = vendors[rnd.Next(0, vendors.Count)];
                        var date = rnd.NextDate(DateTime.Now.AddYears(-1), DateTime.Now);

                        invoiceList.Add(new InvoiceEntity() {
                            No = DateTime.Now.ToString($"DDMMYYYY_{rnd.Next(555)}"),
                            Amount = rnd.NextDecimal(300, 5999),
                            CompanyId = company.Id,
                            VendorId = vendor.Id,
                            Date = date,
                            DueDate = date.AddMonths(1),
                        });
                    }

                    var message = $"Created invoices {invoiceList.Count} at {DateTime.Now}";
                    await notification.SendTextMessage(message);

                    if(invoiceList.Count > 0) {
                        dbContext.Set<InvoiceEntity>().AddRange(invoiceList);
                        await dbContext.SaveChangesAsync();
                    }
                } catch(Exception e) {
                    _logger.LogInformation($"Collector Job: {e.Message}");
                }

                //var emailSender = scope.ServiceProvider.GetService<IEmailSender>();
                // fetch customers, send email, update DB
            }
        }
    }
}
