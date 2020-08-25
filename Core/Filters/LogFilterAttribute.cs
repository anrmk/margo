using System;
using System.Linq;
using System.Security.Claims;

using Core.Data.Dto;
using Core.Extension;
using Core.Services.Managers;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Core.Filters {
    [Obsolete("This attribute is obsolete. Use 'LogAction' or 'ParameterizedLogAction' instead.", false)]
    public class LogFilterAttribute: ActionFilterAttribute {
        private readonly ILogManager _logger;
        private readonly string _par;

        public LogFilterAttribute(ILogManager logger, string par) {
            _logger = logger;
            _par = par;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var logObject = new LogDto() {
                Id = Guid.NewGuid(),
                UserAddress = filterContext.HttpContext.Connection.RemoteIpAddress.ToString(),
                UserId = filterContext.HttpContext.User.GetUserId(),
                UserName = filterContext.HttpContext.User.FindFirstValue(ClaimTypes.Name),
                Method = filterContext.HttpContext.Request.Method,
                Controller = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ControllerName,
                Action = ((ControllerActionDescriptor)filterContext.ActionDescriptor).ActionName,
                Url = $"{filterContext.HttpContext.Request.Path}{filterContext.HttpContext.Request.QueryString.Value}",
                Arguments = JsonConvert.SerializeObject(filterContext.ActionArguments.FirstOrDefault().Value),
                Message = !string.IsNullOrEmpty(_par) ? _par : "Action invoked"
            };

            _logger.LogInfo(JsonConvert.SerializeObject(logObject));
        }
    }
}
