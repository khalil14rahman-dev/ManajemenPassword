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
        DataRepository<PasswordModel> repo = new DataRepository<PasswordModel>("data_password.json");

        public void LoadDataToGrid()
        {
            // Ambil data dari file
            List<PasswordModel> listData = repo.LoadData();

            // Masukkan ke DataGridView
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
            // Membuat objek form input
            FormInputData formInput = new FormInputData();

            // Menampilkan form sebagai Dialog (pop-up yang harus diselesaikan dulu)
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
                    // 1. Ambil data
                    List<PasswordModel> listData = repo.LoadData();

                    // 2. Ambil index baris yang dipilih
                    int index = dataGridView1.CurrentRow.Index;

                    // 3. Hapus dan Simpan
                    listData.RemoveAt(index);
                    repo.SaveData(listData);

                    // 4. Update tampilan
                    LoadDataToGrid();
                    MessageBox.Show("Data berhasil dihapus!");
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Ambil data yang diklik
                PasswordModel dataTerpilih = (PasswordModel)dataGridView1.CurrentRow.DataBoundItem;
                int index = dataGridView1.CurrentRow.Index;

                // Buka form input dengan membawa data tersebut
                FormInputData formEdit = new FormInputData(dataTerpilih, index);
                formEdit.ShowDialog();

                // Refresh tabel setelah ditutup
                LoadDataToGrid();
            }
        }
    }
}
