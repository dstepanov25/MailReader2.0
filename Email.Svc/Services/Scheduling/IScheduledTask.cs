﻿using System.Threading;
using System.Threading.Tasks;

namespace Email.Svc.Services.Scheduling {

    public interface IScheduledTask {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }

}