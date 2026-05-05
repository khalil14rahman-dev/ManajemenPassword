using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class PerformanceChecker
    {
        private AuthManager _auth;

        public PerformanceChecker()
        {
            _auth = new AuthManager();
        }

        // Method untuk mengukur kecepatan simpan log
        public long MeasureSaveLogSpeed(int jumlahData)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < jumlahData; i++)
            {
                _auth.SaveLog($"Performance Test {i}", "Success");
            }

            sw.Stop();
            return sw.ElapsedMilliseconds; // Mengembalikan waktu dalam milidetik
        }
    }
}
