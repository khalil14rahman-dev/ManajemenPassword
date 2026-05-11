using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AuthManager auth = new AuthManager();
            List<LogActivity> dataLog = auth.GetLogs();

            dgvLogs.DataSource = dataLog;

            if (dgvLogs.Columns["Timestamp"] != null)
                dgvLogs.Columns["Timestamp"].HeaderText = "Waktu";

            if (dgvLogs.Columns["Activity"] != null)
                dgvLogs.Columns["Activity"].HeaderText = "Aktivitas";

            if (dgvLogs.Columns["Status"] != null)
                dgvLogs.Columns["Status"].HeaderText = "Status";
        }

        private void FormLog_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
