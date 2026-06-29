using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class LogActivity
    {
        public string Timestamp { get; set; }
        public string Activity { get; set; }
        public string Status { get; set; }

        public LogActivity(string timestamp, string activity, string status)
        {
            this.Timestamp = timestamp;
            this.Activity = activity;
            this.Status = status;
        }
    }
}