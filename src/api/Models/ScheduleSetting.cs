using System;

namespace api.Models
{
    public class ScheduleSetting
    {
        public String Status { get; set; }

        public String Time { get; set; }

        public String TimeZoneOffSet { get; set; }

        public String Type { get; set; }

        public String LastRunDate { get; set; }
    }
}