using System.Security.Claims;
using System.Threading.Tasks;

using AutoMapper;

using Core.Context;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Web.Hubs;

namespace Web.Controllers {
    [Authorize]
    public class BaseController<IController>: Controller {
        protected readonly IController _controller;
        protected readonly ILogger<IController> _logger;
        protected readonly IMapper _mapper;
        protected readonly IHubContext<NotificationHub> _notificationHub;

        protected readonly ApplicationContext _context;

        public string CurrentLanguage => "en";

        public bool IsAjaxRequest => HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public string CurrentUser => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task ClientNotify(string msg) {
            if(_notificationHub != null)
                await _notificationHub.Clients.All.SendAsync("notificationResult", msg);
        }


        [Microsoft.AspNetCore.Mvc.NonAction]
        public override void OnActionExecuting(ActionExecutingContext context) {
            var user = context.HttpContext.User;
            var controller = context.Controller;
            var modelState = context.ModelState;

            var path = Request.Path;
            var host = Request.Host.Value;
            var userName = user.Identity.Name;
            var userIsAuthenticated = user.Identity.IsAuthenticated;



            _logger.LogInformation("OnActionExecuting");
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            _logger.LogInformation("OnActionExecuted");
        }



        public BaseController(ILogger<IController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context) {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _notificationHub = notificationHub;
        }

        public BaseController(ILogger<IController> logger, IMapper mapper, ApplicationContext context) {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public override ViewResult View(string view, object model) {
            ViewBag.Language = CurrentLanguage;
            return base.View(view, model);
        }

        public override ViewResult View(object model) {
            ViewBag.Language = CurrentLanguage;
            return base.View(model);
        }

        public override ViewResult View() {
            ViewBag.Language = CurrentLanguage;
            return base.View();
        }
    }
}