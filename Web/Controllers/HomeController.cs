using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.Models;

namespace Web.Controllers.Mvc {
    public class HomeController: BaseController<HomeController> {
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context) : base(logger, mapper, notificationHub, context) {
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    // [ApiController]
    public class HomeController: ControllerBase {
        private readonly IHubContext<NotificationHub> _notificationHub;

        public HomeController(IHubContext<NotificationHub> notificationHub) {
            _notificationHub = notificationHub;
        }

        [HttpGet]
        [Route("start")]
        public async Task<IActionResult> StartNotify() {
            await _notificationHub.Clients.All.SendAsync("notificationResult", Guid.NewGuid().ToString());
            return Ok("");
        }
    }
    //await _syncDataHubContext.Clients.All.SendAsync("syncNsiResult", result);
}
