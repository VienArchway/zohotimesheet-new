using System;

namespace api.Models
{
    public class ScheduleSetting
    {
        public string Status { get; set; }

        public string Time { get; set; }

        public string TimeZoneOffSet { get; set; }

        public string Type { get; set; }

        public string LastRunDate { get; set; }
    }
}