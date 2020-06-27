using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Core.Services {
    public interface IViewRenderService {
        Task<string> RenderToStringAsync(string viewName, object model);
        Task<string> RenderToStringAsync(string viewName, object model, ViewDataDictionary viewDictionary);
    }

    public class ViewRenderService: IViewRenderService {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        //private readonly IServiceProvider _serviceProvider;
        private readonly IActionContextAccessor _actionContext;

        public ViewRenderService(IActionContextAccessor actionContext, IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider /*IServiceProvider serviceProvider*/) {
            _actionContext = actionContext;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            //_serviceProvider = serviceProvider;
        }

        public async Task<string> RenderToStringAsync(string viewName, object model) {
            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { };
            return await RenderToStringAsync(viewName, model, viewDictionary);
        }

        public async Task<string> RenderToStringAsync(string viewName, object model, ViewDataDictionary viewDictionary) {
            //var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = _actionContext.ActionContext;

            using(var sw = new StringWriter()) {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                if(viewResult.View == null) {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                viewDictionary.Model = model;

                var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw, new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}
