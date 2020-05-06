using System.Security.Principal;

using Core.Context;
using Core.Extensions;
using Core.Services.Business;
using Core.Services.Managers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services {
    public class ServicesConfig {
        public static void Configuration(IServiceCollection services) {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext?.User);

            ///Context
            services.AddTransient<IApplicationContext, ApplicationContext>();
            services.AddTransient<IUserProfileManager, UserProfileManager>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ///Extension Service
            services.AddTransient<IViewRenderService, ViewRenderService>();

            ///Managers
            services.AddTransient<ICompanyManager, CompanyManager>();
            services.AddTransient<ICompanyAddressManager, CompanyAddressManager>();
            services.AddTransient<ISupplierManager, SupplierManager>();
            services.AddTransient<ISupplierAddressManager, SupplierAddressManager>();

            services.AddTransient<IInvoiceManager, InvoiceManager>();

            ///Business
            services.AddTransient<INsiBusinessManager, NsiBusinessManager>();
            services.AddTransient<ICrudBusinessManager, CrudBusinessManager>();
            services.AddTransient<IAccountBusinessService, AccountBusinessService>();
        }
    }
}
