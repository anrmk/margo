using System.Security.Claims;

using AutoMapper;

using Core.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    [Authorize]
    public class BaseController<IController>: Controller {
        protected readonly IController _controller;
        protected readonly ILogger<IController> _logger;
        protected readonly IMapper _mapper;
        protected readonly ApplicationContext _context;

        public string CurrentLanguage => "en";

        public string CurrentUser => User.FindFirst(ClaimTypes.NameIdentifier).Value;

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