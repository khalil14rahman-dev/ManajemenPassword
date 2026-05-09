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
    public partial class FormDashboard : Form
    {
        //generic
        DataRepository<PasswordModel> repo = new DataRepository<PasswordModel>("data_password.json");

        public void LoadDataToGrid()
        {
            //generic
            List<PasswordModel> listData = repo.LoadData();

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listData;

            
            dataGridView1.Columns["NamaAplikasi"].HeaderText = "Aplikasi";
            dataGridView1.Columns["Username"].HeaderText = "User / Email";
            dataGridView1.Columns["Password"].HeaderText = "Password";
        }
        public FormDashboard()
        {
            InitializeComponent();
            LoadDataToGrid();
        }



        private void btnTambah_Click(object sender, EventArgs e)
        {
            FormInputData formInput = new FormInputData();

            formInput.ShowDialog();
            LoadDataToGrid();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FormSetting formSet = new FormSetting();
            formSet.ShowDialog();
        }

        private void btnGenerateDashboard_Click(object sender, EventArgs e)
        {

        }

        private void hapusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var confirm = MessageBox.Show("Apakah yakin ingin menghapus akun ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    List<PasswordModel> listData = repo.LoadData();

                    int index = dataGridView1.CurrentRow.Index;

                    listData.RemoveAt(index);
                    repo.SaveData(listData);

                    LoadDataToGrid();
                    MessageBox.Show("Data berhasil dihapus!");
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                PasswordModel dataTerpilih = (PasswordModel)dataGridView1.CurrentRow.DataBoundItem;
                int index = dataGridView1.CurrentRow.Index;

                FormInputData formEdit = new FormInputData(dataTerpilih, index);
                formEdit.ShowDialog();

                LoadDataToGrid();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Membuat instance form log baru
            FormLog logWindow = new FormLog();

            // Menampilkan sebagai Pop-up (Modal)
            logWindow.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtJumlahLog.Text, out int jumlah))
            {
                PerformanceChecker checker = new PerformanceChecker();

                // Ambil jumlah dari TextBox
                long timeTaken = checker.MeasureSaveLogSpeed(jumlah);

                MessageBox.Show($"Hasil Uji Performa:\n" +
                                $"Input: {jumlah} data log\n" +
                                $"Waktu: {timeTaken} ms", "Performance Report");
            }
            else
            {
                MessageBox.Show("Dra, masukin angka yang bener dong! hwhwhw", "Input Error");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtJumlahLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
