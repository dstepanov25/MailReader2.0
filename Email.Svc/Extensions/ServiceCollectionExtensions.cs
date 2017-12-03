using System;
using System.Threading.Tasks;
using Email.Svc.Services.Scheduling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Email.Svc.Extensions {

    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddScheduler(this IServiceCollection services) {
            return services.AddSingleton<IHostedService, SchedulerHostedService>();
        }

        public static IServiceCollection AddScheduler(this IServiceCollection services,
            EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler) {
            return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider => {
                var instance = new SchedulerHostedService(serviceProvider.GetServices<IScheduledTask>());
                instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                return instance;
            });
        }
    }

}