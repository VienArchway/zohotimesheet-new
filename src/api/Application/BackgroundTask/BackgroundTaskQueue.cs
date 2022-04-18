using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using api.Application.Interfaces;

namespace api.Application
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private readonly SemaphoreSlim signal = new SemaphoreSlim(0);

        bool IBackgroundTaskQueue.Status { get; set; }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            workItems.Enqueue(workItem);
            signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken).ConfigureAwait(false);
            workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}