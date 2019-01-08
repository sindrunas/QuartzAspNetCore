using Microsoft.AspNetCore.Builder;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzAspNetCore
{
    public static class QuartzExtensions
    {
        public static void UseQuartz(this IApplicationBuilder app, Action<Quartz> configuration)
        {
            // Job Factory through IOC container
            var jobFactory = (IJobFactory)app.ApplicationServices.GetService(typeof(IJobFactory));
            // Set job factory
            Quartz.Instance.UseJobFactory(jobFactory);

            // Run configuration
            configuration.Invoke(Quartz.Instance);
            // Run Quartz
            Quartz.Start();
        }
    }
}
