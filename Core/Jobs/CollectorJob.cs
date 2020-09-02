using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;
using Core.Data.Dto;
using Core.Data.Entities;
using Core.Extension;
using Core.Services;
using Core.Services.Business;
using Core.Services.Integration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Quartz;

namespace Core.Jobs {
    [DisallowConcurrentExecution]
    public class CollectorJob: IJob {
        private readonly IMapper _mapper;
        private readonly ILogger<CollectorJob> _logger;
        private readonly IServiceProvider _provider;

        public CollectorJob(ILogger<CollectorJob> logger, IMapper mapper, IServiceProvider provider) {
            _mapper = mapper;
            _logger = logger;
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context) {
            using(var scope = _provider.CreateScope()) {
                var dbContext = scope.ServiceProvider.GetService<ApplicationContext>();
                //var notification = scope.ServiceProvider.GetService<INotifyService>();

                try {
                    var accounts = await dbContext.Set<UccountEntity>()
                        .Include(x => x.Fields)
                        .Where(x => x.VendorId == new Guid("40F1266A-B2C6-45AC-5976-08D84DDE544D")) //Toyota Finance
                        .OrderBy(x => x.VendorId)
                        .ToListAsync();

                    var invoiceList = new List<InvoiceDto>();

                    foreach(var account in accounts) {
                        var service = scope.ServiceProvider.GetService<IToyotaFinancialService>();
                        var result = await service.Execute(account);

                        invoiceList.AddRange(result);
                    }

                    var message = $"Created invoices {invoiceList.Count} at {DateTime.Now}";
                    //await notification.SendTextMessage(message);

                    if(invoiceList.Count > 0) {
                        dbContext.Set<InvoiceEntity>().AddRange(_mapper.Map<InvoiceEntity>(invoiceList));
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
