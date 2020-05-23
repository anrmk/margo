
using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Context;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Web.Hubs;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    public class PaymentController: BaseController<PaymentController> {
        private readonly ICrudBusinessManager _crudBusinessManager;

        public PaymentController(ILogger<PaymentController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context,
            ICrudBusinessManager crudBusinessManager) : base(logger, mapper, notificationHub, context) {
            _crudBusinessManager = crudBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }

        public async Task<IActionResult> Create(long id) {
            var item = await _crudBusinessManager.GetInvoice(id);
            if(item == null) {
                return NotFound();
            }

            var rnd = new Random();

            var model = new PaymentViewModel() {
                Date = DateTime.Now,
                InvoiceId = item.Id,
                InvoiceNo = item.No,
                No = string.Format("{0}-{1}-{2}", DateTime.Now.ToString("yyyyMMdd") , rnd.Next(0, 99), Guid.NewGuid().ToString().Substring(0,5))
            };

            return View(model);
        }
    }
}
