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

            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listData;

            if (dataGridView1.Columns.Contains("colAplikasi"))
            {
                dataGridView1.Columns["colAplikasi"].DataPropertyName = "NamaAplikasi";
            }

            if (dataGridView1.Columns.Contains("colUsername"))
            {
                dataGridView1.Columns["colUsername"].DataPropertyName = "Username";
            }

            if (dataGridView1.Columns.Contains("colPassword"))
            {
                dataGridView1.Columns["colPassword"].DataPropertyName = "Password";
            }
        }

        private AuthManager auth = AuthManager.GetInstance();
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
            FormLog logWindow = new FormLog();
            logWindow.ShowDialog();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtJumlahLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = (PasswordModel)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                FormDetailPassword formDetail = new FormDetailPassword(data.NamaAplikasi, data.Username, data.Password);
                formDetail.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)

        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // CLEAN CODE: Memakai objek pusat '_auth' yang konsisten, bukan membuat objek 'new' tiruan
            auth.Logout();
            auth.SaveLog("Logout User", "Success");

            // CLEAN CODE: Menggunakan MessageBoxIcon untuk kualitas UX standar industri
            MessageBox.Show("Anda telah berhasil logout.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Form1 loginForm = new Form1();
            loginForm.Show();

            this.Close();
        }
    }
}