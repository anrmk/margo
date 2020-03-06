using System.Diagnostics;
using AutoMapper;
using Core.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Web.Models;

namespace Web.Controllers {
    public class HomeController: BaseController<HomeController> {
        public HomeController(ILogger<HomeController> logger, IMapper mapper, ApplicationContext context) : base(logger, mapper, context) {
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
