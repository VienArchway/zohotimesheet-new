using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using api.Application.Interfaces;

namespace api.Application
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger logger;

        private readonly IBackgroundTaskQueue taskQueue;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILoggerFactory loggerFactory)
        {
            this.taskQueue = taskQueue;
            this.logger = loggerFactory.CreateLogger<QueuedHostedService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Queued Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await taskQueue.DequeueAsync(stoppingToken).ConfigureAwait(false);

                try
                {
                    taskQueue.Status = true;
                    await workItem(stoppingToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
                finally
                {
                    taskQueue.Status = false;
                }
            }

            logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
}