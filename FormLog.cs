using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
            LoadLogToTable();
        }

        private void LoadLogToTable()
        {
            try
            {
                AuthManager auth = AuthManager.GetInstance();
                List<LogActivity> dataLog = auth.GetLogs();

                dgvLogs.AutoGenerateColumns = true; 
                dgvLogs.DataSource = null;
                dgvLogs.DataSource = dataLog;

                if (dgvLogs.Columns["Waktu"] != null)
                    dgvLogs.Columns["Waktu"].HeaderText = "Waktu Aktivitas";

                if (dgvLogs.Columns["Aktivitas"] != null)
                    dgvLogs.Columns["Aktivitas"].HeaderText = "Detail Aktivitas";

                if (dgvLogs.Columns["Status"] != null)
                    dgvLogs.Columns["Status"].HeaderText = "Status";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat log aktivitas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }
}