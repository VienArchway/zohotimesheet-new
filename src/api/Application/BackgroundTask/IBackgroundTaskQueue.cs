using System;
using System.Threading;
using System.Threading.Tasks;

namespace api.Application.Interfaces
{
    public interface IBackgroundTaskQueue
    {
        bool Status { get; set; }

        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}