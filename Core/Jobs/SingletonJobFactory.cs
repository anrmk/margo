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
            try {
                return _serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            } catch(Exception e) {
                Console.WriteLine($"SingletonJobFactory Exceptino: {e.Message}");
                return null;
            }
        }

        public void ReturnJob(IJob job) {
        }
    }
}
