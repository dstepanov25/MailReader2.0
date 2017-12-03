using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Email.Svc.Services.Scheduling
{
    public class JobScheduler
    {
        public  static async Task Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<ReadEmailsJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
