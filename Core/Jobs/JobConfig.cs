using Core.HostedService;
using Core.JobFactory;

using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Core.Jobs {
    public class JobConfig {
        public static void Configuration(IServiceCollection services) {
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<NotifyJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(NotifyJob),
                cronExpression: "0/5 * * * * ?")); // run every 5 seconds

            // Add Hosted Service
            services.AddHostedService<QuartzHostedService>();

        }
    }
}
