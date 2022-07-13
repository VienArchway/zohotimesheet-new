using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;
using Newtonsoft.Json;

namespace api.Application
{
    public class ZohoHostedService : IHostedService, IDisposable
    {
        private readonly ILogger logger;

        private readonly IServiceProvider services;

        private readonly IConfiguration configuration;

        private readonly String scheduleSettingFile;

        private Timer timer;

        private ScheduleSetting schedule;

        public ZohoHostedService(IServiceProvider services, ILogger<ZohoHostedService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.services = services;
            this.configuration = configuration;
            this.scheduleSettingFile = configuration.GetValue<String>("Adls:ScheduleSettingFile");
        }

        public virtual async Task<ScheduleSetting> StartAsync(ScheduleSetting condition)
        {
            var currentStatus = await GetStatusAsync().ConfigureAwait(false);
            schedule = condition;
            schedule.LastRunDate = currentStatus.LastRunDate;
            await StartAsync(new CancellationToken(true)).ConfigureAwait(false);
            return await UploadStatusToAdslAsync("running").ConfigureAwait(false);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (schedule == null)
            {
                schedule = await GetStatusAsync().ConfigureAwait(false);
            }

            var offsetTime = String.IsNullOrEmpty(schedule.TimeZoneOffSet) ? "-540" : schedule.TimeZoneOffSet; //japan locale zone time +9:00
            var offsetTimeValue = double.Parse(offsetTime);
            if (DateTimeOffset.Now.Offset.TotalMinutes * -1 == offsetTimeValue)
            {
                offsetTimeValue = 0;
            }
            else
            {
                offsetTimeValue = offsetTimeValue - (DateTimeOffset.Now.Offset.TotalMinutes * -1);
            }

            if (!String.IsNullOrEmpty(schedule.Time) && !String.IsNullOrEmpty(schedule.Type))
            {
                logger.LogInformation("Zoho Background Service is starting.");

                var time = 0.0;
                if (!"TIME".Equals(schedule.Type.ToUpper()))
                {
                    time = double.Parse(schedule.Time);
                }

                timer?.Change(Timeout.Infinite, 0);
                timer?.Dispose();

                switch (schedule.Type.ToUpper())
                {
                    case "DAYOFWEEK":
                        var todayIndexOfWeek = (int)DateTime.Now.DayOfWeek;
                        var isTimeInThisWeek = todayIndexOfWeek <= time;
                        var dueDay = isTimeInThisWeek ? time - todayIndexOfWeek : (time - todayIndexOfWeek) + 7;
                        var dueDate = DateTime.Now.Add(TimeSpan.FromDays(dueDay));
                        var dueDateTime = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, 8, 0, 0).AddMinutes(offsetTimeValue);
                        var dueTime = dueDateTime > DateTime.Now ? dueDateTime - DateTime.Now : dueDateTime.AddDays(7) - DateTime.Now;
                        timer = new Timer(DoWork, null, dueTime, TimeSpan.FromDays(7));
                        break;
                    case "TIME":
                        var hourTime = schedule.Time.Split(":"); // Time format: HH:mm TZD (ex: 11:50:+540)
                        if (hourTime.Length < 2)
                        {
                            throw new ArgumentException("Time is Invalid!");
                        }

                        var hour = hourTime[0];
                        var minute = hourTime[1];
                        var now = DateTime.Now;
                        var nextDate = now > new DateTime(now.Year, now.Month, now.Day, int.Parse(hour), int.Parse(minute), 0).AddMinutes(offsetTimeValue) ?
                            DateTime.Now.AddDays(1) : now;
                        dueDateTime = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, int.Parse(hour), int.Parse(minute), 0).AddMinutes(offsetTimeValue);
                        dueTime = dueDateTime - DateTime.Now;
                        timer = new Timer(DoWork, null, dueTime, TimeSpan.FromDays(1));
                        break;
                    case "MINUTE":
                        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(time));
                        break;
                    case "HOUR":
                        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(time));
                        break;
                    default:
                        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(time));
                        break;
                }
            }
        }

        public virtual async Task<ScheduleSetting> StopAsync()
        {
            var currentStatus = await GetStatusAsync().ConfigureAwait(false);
            schedule = currentStatus;
            await StopAsync(new CancellationToken(true)).ConfigureAwait(false);
            return await UploadStatusToAdslAsync("stopped").ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Zoho Background Service is stopping.");

            timer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public virtual async Task<ScheduleSetting> GetStatusAsync()
        {
            using (var scope = services.CreateScope())
            {
                var adlsClient = scope.ServiceProvider.GetRequiredService<IAdlsClient>();
                var isExistSettingFile = adlsClient.CheckExist(scheduleSettingFile);
                if (!isExistSettingFile)
                {
                    await adlsClient.CreateFileAsync(scheduleSettingFile).ConfigureAwait(false);
                }

                var currentStatusContent = await adlsClient.DownloadAsync(scheduleSettingFile).ConfigureAwait(false);
                var status = JsonConvert.DeserializeObject<ScheduleSetting>(currentStatusContent);

                if (status == null)
                {
                    status = new ScheduleSetting()
                    {
                        Status = "stopped",
                        Time = configuration.GetValue<String>("Schedule:Time"),
                        Type = configuration.GetValue<String>("Schedule:Type"),
                        TimeZoneOffSet = configuration.GetValue<String>("Schedule:TimeZoneOffSet")
                    };
                }

                return status;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            timer?.Dispose();
        }

        private void DoWork(object state)
        {
            logger.LogInformation("Zoho Background Service is running.");

            using (var scope = services.CreateScope())
            {
                var adlsService = scope.ServiceProvider.GetRequiredService<IAdlsService>();
                var desiredDay = DayOfWeek.Monday;
                var offsetAmount = (int)desiredDay - (int)DateTime.Now.DayOfWeek;
                var startDate = schedule.Type.Equals("DAYOFWEEK") ? DateTime.Now.AddDays(-7 + offsetAmount) : DateTime.Now.AddDays(-2);
                adlsService.TransferAdlsAsync(new LogWorkSearchParameter() { StartDate = startDate.Date, EndDate = DateTime.Now.Date, SprintTypeId = 2 }, "Schedule").Wait();

                schedule.LastRunDate = DateTime.UtcNow.ToString();
                UploadStatusToAdslAsync("running").Wait();
                logger.LogInformation("Zoho Background Service finished.");
            }
        }

        private async Task<ScheduleSetting> UploadStatusToAdslAsync(String status)
        {
            using (var scope = services.CreateScope())
            {
                var adlsClient = scope.ServiceProvider.GetRequiredService<IAdlsClient>();

                var newStatus = new ScheduleSetting()
                {
                    Time = schedule.Time,
                    Type = schedule.Type,
                    Status = status,
                    LastRunDate = schedule.LastRunDate,
                    TimeZoneOffSet = schedule.TimeZoneOffSet
                };
                var contentSetting = JsonConvert.SerializeObject(newStatus);
                await adlsClient.UploadAsync(contentSetting, scheduleSettingFile).ConfigureAwait(false);
                return newStatus;
            }
        }
    }
}