using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QuartzAspNetCore
{
    public class QuartzJobFactory : IJobFactory
    {
        protected readonly IServiceProvider serviceProvider;
        private ConcurrentDictionary<IJob, IServiceScope> scopes = new ConcurrentDictionary<IJob, IServiceScope>();

        public QuartzJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var scope = serviceProvider.CreateScope();
                var job = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;

                scopes.TryAdd(job, scope);

                return job;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error has ocurred instantiating a new job: {ex.Message}. StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            try
            {
                (job as IDisposable)?.Dispose();

                if (scopes.TryRemove(job, out IServiceScope scope))
                    scope.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error has ocurred returning a job: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }
    }
}
