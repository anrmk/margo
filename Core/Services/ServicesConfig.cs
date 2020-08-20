using System.Security.Principal;

using Core.Context;
using Core.Data.Entities;
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

            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext?.User);

            ///Extension Service
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<ILogManager, LogManager>();

            ///Context
            services.AddTransient<IApplicationContext, ApplicationContext>();

            services.AddTransient<IUserProfileManager, UserProfileManager>();
            services.AddTransient<IUserRequestManager, UserRequestManager>();
       
            services.AddTransient<IAspNetUserDenyAccessCompanyManager, AspNetUserDenyAccessCompanyManager>();
            services.AddTransient<IAspNetUserDenyAccessCategoryManager, AspNetUserDenyAccessCategoryManager>();
            services.AddTransient<ILogManager, LogManager>();

            ///Extension Service
            services.AddTransient<IViewRenderService, ViewRenderService>();

            ///Managers
            services.AddTransient<IPersonManager, PersonManager>();

            services.AddTransient<ICompanyManager, CompanyManager>();
            services.AddTransient<ICompanySectionManager, CompanySectionManager>();
            services.AddTransient<ICompanyDataManager, CompanyDataManager>();

            services.AddTransient<ICategoryManager, CategoryManager>();
            services.AddTransient<ICategoryFieldManager, CategoryFieldManager>();

            services.AddTransient<IVendorManager, VendorManager>();
            services.AddTransient<IVendorFieldManager, VendorFieldManager>();

            services.AddTransient<IUccountManager, UccountManager>();
            services.AddTransient<IUccountServiceManager, UccountServiceManager>();
            services.AddTransient<IUccountServiceFieldManager, UccountServiceFieldManager>();
            services.AddTransient<IUccountVendorFieldManager, UccountVendorFieldManager>();

            services.AddTransient<IInvoiceManager, InvoiceManager>();
            services.AddTransient<IPaymentManager, PaymentManager>();

            /////Business
            services.AddTransient<IAccountBusinessManager, AccountBusinessManager>();
            services.AddTransient<IPersonBusinessManager, PersonBusinessManager>();
            services.AddTransient<ICompanyBusinessManager, CompanyBusinessManager>();
            services.AddTransient<ICategoryBusinessManager, CategoryBusinessManager>();
            services.AddTransient<IUccountBusinessManager, UccountBusinessManager>();
            services.AddTransient<IVendorBusinessManager, VendorBusinessManager>();
            services.AddTransient<IInvoiceBusinessManager, InvoiceBusinessManager>();
            services.AddTransient<IPaymentBusinessManager, PaymentBusinessManager>();

            ///Grants by user
            services.AddTransient<GrantManager<UccountEntity>, UccountGrantManager>();
            services.AddTransient<GrantManager<CompanyEntity>, CompanyGrantManager>();
            services.AddTransient<GrantManager<CategoryEntity>, CategoryGrantManager>();
            services.AddTransient<UccountServiceGrantManager>();
        }
    }
}
