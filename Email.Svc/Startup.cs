using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Email.EntityFramework;
using Email.Svc.Constants;
using Email.Svc.Services.Mailbox;
using Email.Svc.Services.Scheduling;
using Email.Svc.Services.Settings;
using Email.Svc.Services.Suppliers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Email.Svc {

    public class Startup {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            //JobScheduler.Start();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().AddJsonOptions(opts => {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opts.SerializerSettings.Converters.Add(new StringEnumConverter {
                    CamelCaseText = true
                });
            });


            // read Environment variables
            var mongodbconnectionName =
                Environment.GetEnvironmentVariable(EnvironmentVariables.ConnectionNameMongoDb,
                                                   EnvironmentVariableTarget.Machine);
            if (string.IsNullOrEmpty(mongodbconnectionName)) {
                mongodbconnectionName = "MongoDefault";
            }
            var mongodbConnectionString = Configuration.GetConnectionString(mongodbconnectionName);
            var mongoUrl = new MongoUrl(mongodbConnectionString);

            services.AddSingleton<IMongoClient>(provider => new MongoClient(mongoUrl));
            services.AddSingleton<IMongoDatabase>(provider => provider.GetService<IMongoClient>()
                                                      .GetDatabase(mongoUrl.DatabaseName));

            services.AddSingleton<IMailService, MailService>();
            services.AddSingleton<ISettingsService, SettingsService>();

            services.AddEntityFrameworkSqlite()
                    .AddDbContext<EmailContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")),ServiceLifetime.Singleton);
            services.AddSingleton<ISupplierService, SupplierService>();

            services.UseQuartz(typeof(ReadEmailsJob));

            //services.AddSingleton<IScheduledTask, CheckMailboxTask>();

            //services.AddScheduler((sender, args) =>
            //{
            //    Console.Write(args.Exception.Message);
            //    args.SetObserved();
            //});
            ScheduleJobs();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();


            var scheduler = serviceProvider.GetService<IScheduler>();

            QuartzServicesUtilities.StartJob<ReadEmailsJob>(scheduler, new TimeSpan(0,1,0));


            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope()) {
                var db = serviceScope.ServiceProvider.GetService<EmailContext>();
                db.Database.EnsureCreated();
            }
            


                //var _settingsService = serviceProvider.GetService<ISettingsService>();
                //var _mailService = serviceProvider.GetService<IMailService>();

                //var emailAccounts = _settingsService.GetEmailAccounts().Result;
                //foreach (var emailAccount in emailAccounts)
                //{
                //    _mailService.GetMessages(emailAccount);
                //}
            }

        private async Task ScheduleJobs() {
            // Grab the Scheduler instance from the Factory
            var props = new NameValueCollection {
                {"quartz.serializer.type", "binary"},
                {"quartz.threadPool.threadCount", "1"},
                {"quartz.scheduler.instanceName", "MailReaderSimpleScheduler"}
            };
            var factory = new StdSchedulerFactory(props);
            var scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our ReadEmailsJob class
            var job = JobBuilder.Create<ReadEmailsJob>().WithIdentity("job1", "group1").Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            var trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever()).Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }
    }

    public static class QuartzExtensions {
        public static void UseQuartz(this IServiceCollection services, params Type[] jobs) {
            services.AddSingleton<IJobFactory, QuartzJonFactory>();
            var xxx = jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Singleton))
                .First();
            services.Add(xxx);

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });

        }
            
    }

    public class QuartzJonFactory : IJobFactory {
        private readonly IServiceProvider _serviceProvider;

        public QuartzJonFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
            var jobDetail = bundle.JobDetail;

            var job = (IJob) _serviceProvider.GetService(jobDetail.JobType);
            return job;
        }

        public void ReturnJob(IJob job) {
        }
    }

    public static class QuartzServicesUtilities {
        public static void StartJob<TJob>(IScheduler scheduler, TimeSpan runInterval) where TJob : IJob {
            var jobName = typeof(TJob).FullName;

            var job = JobBuilder.Create<TJob>().WithIdentity(jobName).Build();

            var trigger = TriggerBuilder.Create().WithIdentity($"{jobName}.trigger").StartNow()
                .WithSimpleSchedule(scheduleBuilder => scheduleBuilder.WithInterval(runInterval).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }

}