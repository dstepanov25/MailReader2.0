using System;
using System.Collections.Specialized;
using System.Linq;
using Email.Svc.Services.Scheduling;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Email.Svc {

    public class Program {
        private static IScheduler _scheduler; // add this field

        public static void Main(string[] args) {
            BuildWebHost(args).Run();

            //StartScheduler(); // add this line
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
            .Build();

        // add this method
        private static void StartScheduler()
        {
            var properties = new NameValueCollection
            {
                // json serialization is the one supported under .NET Core (binary isn't)
                ["quartz.serializer.type"] = "json",

                // the following setup of job store is just for example and it didn't change from v2
                // according to your usage scenario though, you definitely need 
                // the ADO.NET job store and not the RAMJobStore.
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.useProperties"] = "false",
                ["quartz.jobStore.dataSource"] = "default",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                //["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
                //["quartz.dataSource.default.provider"] = "SqlServer-41", // SqlServer-41 is the new provider for .NET Core
                //["quartz.dataSource.default.connectionString"] = @"Server=(localdb)\MSSQLLocalDB;Database=Quartz;Integrated Security=true"
            };

            var schedulerFactory = new StdSchedulerFactory(properties);
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.Start().Wait();

            var userEmailsJob = JobBuilder.Create<ReadEmailsJob>()
                .WithIdentity("SendUserEmails")
                .Build();
            var userEmailsTrigger = TriggerBuilder.Create()
                .WithIdentity("UserEmailsCron")
                .StartNow()
                .WithCronSchedule("0 0 17 ? * MON,TUE,WED")
                .Build();

            _scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger).Wait();

            
        }

        //public static void UseQuartz(this IServiceCollection services, params Type[] jobs)
        //{
        //    services.AddSingleton<IJobFactory, QuartzJonFactory>();
        //    var xxx = jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton));
        //    services.Add(xxx);

        //    services.AddSingleton(provider =>
        //    {
        //        var schedulerFactory = new StdSchedulerFactory();
        //        var scheduler = schedulerFactory.GetScheduler();
        //        scheduler.JobFactory = provider.GetService<IJobFactory>();
        //        scheduler.Start();
        //        return scheduler;
        //    });
        //}
    }



}