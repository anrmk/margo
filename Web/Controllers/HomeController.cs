using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using Core.Data.Dto;
using Core.Extension;
using Core.Filters;
using Core.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers.Mvc {
    [Authorize]
    [LogAction]
    public class HomeController: BaseController<HomeController> {
        private readonly IAccountBusinessManager _accountBusinessManager;
        private readonly ICompanyBusinessManager _companyBusinessManager;
        
        public HomeController(ILogger<HomeController> logger, IMapper mapper,
            IAccountBusinessManager accountBusinessManager,
            ICompanyBusinessManager companyBusinessManager) : base(logger, mapper) {
            _accountBusinessManager = accountBusinessManager;
            _companyBusinessManager = companyBusinessManager;
        }

        public async Task<IActionResult> Index() {
            //throw new Exception("Some Exceprion ");
            // LogManager.Configuration.Install(new NLog.Config.InstallationContext(Console.Out));
            //_logger.LogInformation(new EventId(33, name: "Index33"), "{shopitem} added to basket by {user}", new { Id = 6, Name = "Jacket", Color = "Orange" }, "Kenny");
            //_logger.LogError("Error on HomeController");
            //_logger.LogDebug("Debug on HomeController");
            //_logger.LogTrace("Trace on HomeController");
            //_logger.LogCritical("Critical on HomeController");
            //_logger.LogWarning("Warning on HomeController");

            var allCompanies = await _companyBusinessManager.GetCompanies();
            var favouriteCompanies = await _accountBusinessManager.GetFavouriteCompanies(User.GetUserId());
            var favouriteCompanyIds = favouriteCompanies.Select(x => x.CompanyId).ToHashSet();

            ViewBag.Companies = _mapper.Map<List<CompanyViewModel>>(allCompanies.Where(x => !favouriteCompanyIds.Contains(x.Id)));

            return View(_mapper.Map<List<AspNetUserCompanyFavouriteViewModel>>(favouriteCompanies));
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
    [ApiController]
    [Authorize]
    [LogAction]
    public class HomeController: ControllerBase {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IMapper _mapper;
        private readonly IAccountBusinessManager _accountBusinessManager;

        public HomeController(IHubContext<NotificationHub> notificationHub, IMapper mapper, IAccountBusinessManager accountBusinessManager) {
            _notificationHub = notificationHub;
            _mapper = mapper;
            _accountBusinessManager = accountBusinessManager;
        }

        [HttpGet]
        [Route("start")]
        public async Task<IActionResult> StartNotify() {
            await _notificationHub.Clients.All.SendAsync("notificationResult", Guid.NewGuid().ToString());
            return Ok("");
        }

        [HttpPut("UpdateAspNetUserFavouriteCompanies", Name = "UpdateAspNetUserFavouriteCompanies")]
        public async Task<IActionResult> UpdateAspNetUserFavouriteCompanies([FromBody] List<AspNetUserCompanyFavouriteViewModel> model) {
            try {
                if(model == null)
                    throw new Exception("No data!");

                var item = await _accountBusinessManager.UpdateFavouriteCompanies(User.GetUserId(), model.Select((x, i) => new AspNetUserCompanyFavouriteDto { Sort = i, CompanyId = x.CompanyId }));

                if(item == null)
                    return NotFound();

                return Ok(_mapper.Map<List<AspNetUserCompanyFavouriteViewModel>>(item));
            } catch(Exception er) {
                return BadRequest(er.Message ?? er.StackTrace);
            }
        }
    }

    //await _syncDataHubContext.Clients.All.SendAsync("syncNsiResult", result);
}
