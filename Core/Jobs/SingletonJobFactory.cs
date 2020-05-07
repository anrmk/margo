using System;

using Quartz;
using Quartz.Spi;

namespace Core.JobFactory {
    public class SingletonJobFactory: IJobFactory {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
            //return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            return _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job) {

        }
    }
}
