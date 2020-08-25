using System.Security.Claims;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    [Authorize]
    public class BaseController<IController>: Controller {
        protected readonly IController _controller;
        protected readonly ILogger<IController> _logger;
        protected readonly IMapper _mapper;
        // protected readonly IHubContext<NotificationHub> _notificationHub;

        //protected readonly ApplicationContext _context;

        public string CurrentLanguage => "en";

        public bool IsAjaxRequest => HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        public string CurrentUser => User.FindFirst(ClaimTypes.NameIdentifier).Value;
        /*
        public async Task ClientNotify(string msg) {

            if(_notificationHub != null)
                await _notificationHub.Clients.All.SendAsync("notificationResult", msg);

        }
 */

        public override void OnActionExecuted(ActionExecutedContext context) {
            var userName = context.HttpContext.User?.Identity.Name;

            if(context.Exception != null) {
                _logger.LogError("User {user} activity exception", userName);
            }
        }

        //public BaseController(IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context) {
        //    _mapper = mapper;
        //    _context = context;
        //   // _notificationHub = notificationHub;
        //}

        public BaseController(ILogger<IController> logger, IMapper mapper) {
            _logger = logger;
            _mapper = mapper;
            // _context = context;
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

    //[ApiController]
    //public class BaseApiController<IController>: ControllerBase {
    //    protected readonly IMapper _mapper;

    //    public BaseApiController(IMapper mapper) {
    //        _mapper = mapper;
    //    }
    //}
}