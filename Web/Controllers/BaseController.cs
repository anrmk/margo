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

        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context) {
            //var user = context.HttpContext.User;
            //var controller = context.Controller;
            //var modelState = context.ModelState;

            //var path = Request.Path;
            //var host = Request.Host.Value;
            //var userName = user.Identity.Name;
            //var userIsAuthenticated = user.Identity.IsAuthenticated;


            //_logger.LogInformation(new EventId(33, name: "Index33"), "{shopitem} added to basket by {user}", new { Id = 6, Name = "Jacket", Color = "Orange" }, "Kenny");

            //_logger.LogInformation("{user} Activity log", user?.Identity.Name);
            //  base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            var userName = context.HttpContext.User?.Identity.Name;

            if(context.Exception != null)
                _logger.LogError("User {user} make activity ", userName);
            else
                _logger.LogInformation("User {user} make activity ", userName);

            //base.OnActionExecuted(context);
            // _logger.LogInformation("OnActionExecuted");
        }



        //public BaseController(ILogger<IController> logger, IMapper mapper, IHubContext<NotificationHub> notificationHub, ApplicationContext context) {
        //    _logger = logger;
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
    //    protected readonly ILogger<IController> _logger;
    //    protected readonly IMapper _mapper;

    //    public BaseApiController(ILogger<IController> logger, IMapper mapper) {
    //        _logger = logger;
    //        _mapper = mapper;
    //    }
    //}
}