using Core.HostedService;
using Core.JobFactory;

using Microsoft.Extensions.DependencyInjection;

using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Core.Jobs {
    public class JobSchedulerConfig {
        public static void Configuration(IServiceCollection services) {
            // Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<NotifyJob>();
            services.AddSingleton<CollectorJob>();
            //services.AddSingleton(new JobSchedule(jobType: typeof(NotifyJob), cronExpression: "0 0/1 * * * ?")); // run every 10 seconds
            services.AddSingleton(new JobSchedule(jobType: typeof(CollectorJob), cronExpression: "0 0/1 * * * ?")); // run every 30 minutes

            // Add Hosted Service
            services.AddHostedService<QuartzHostedService>();
        }
    }
}
