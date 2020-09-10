using Core.Services.Integration;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Services {
    public class SeleniumServiceConfig {
        public static void Configuration(IServiceCollection services) {
            //services.AddTransient<ISeleniumService, SeleniumService>();
            services.AddTransient<IToyotaFinancialService, ToyotaFinancialService>();
        }
    }
}

