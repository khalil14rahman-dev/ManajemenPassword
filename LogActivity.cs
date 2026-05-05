using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class LogActivity
    {
        public DateTime Timestamp { get; set; }
        public string Activity { get; set; } // Contoh: Login, Akses Password, Ubah Master Key
        public string Status { get; set; }   // Contoh: Berhasil atau Gagal

        // Constructor biar gampang pas bikin log baru
        public LogActivity(string activity, string status)
        {
            this.Timestamp = DateTime.Now;
            this.Activity = activity;
            this.Status = status;
        }

    }
}
