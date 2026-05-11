using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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

            // 1. TAMBAHKAN INI: Agar kolom manual kamu tidak tertumpuk kolom otomatis
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listData;

            // 2. HUBUNGKAN KOLOM: Pastikan nama di dalam kurung [ ] sesuai dengan properti di PasswordModel
            // colAplikasi adalah Name yang kamu buat di "Edit Columns" tadi
            if (dataGridView1.Columns.Contains("colAplikasi"))
            {
                dataGridView1.Columns["colAplikasi"].DataPropertyName = "NamaAplikasi";
            }

            // Username dan Password tetap kita hubungkan ke kolom yang Visible = False 
            // agar datanya "terbawa" tapi tidak terlihat di layar
            if (dataGridView1.Columns.Contains("colUsername"))
            {
                dataGridView1.Columns["colUsername"].DataPropertyName = "Username";
            }

            if (dataGridView1.Columns.Contains("colPassword"))
            {
                dataGridView1.Columns["colPassword"].DataPropertyName = "Password";
            }
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
            // Pre-kondisi: harus ada baris yang dipilih
            Debug.Assert(dataGridView1.CurrentRow != null, "Kontrak Gagal: CurrentRow tidak boleh null saat menghapus.");

            if (dataGridView1.CurrentRow != null)
            {
                string namaAplikasi = dataGridView1.CurrentRow.Cells[0].Value?.ToString() ?? "Unknown";

                var confirm = MessageBox.Show($"Apakah yakin ingin menghapus akun {namaAplikasi}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        List<PasswordModel> listData = repo.LoadData();
                        int index = dataGridView1.CurrentRow.Index;

                        // Invariant agar index yang didapat sesuai dengan jumlah data di list
                        Debug.Assert(index >= 0 && index < listData.Count, "Kontrak Gagal: Indeks di luar jangkauan list data.");

                        // Hapus data
                        listData.RemoveAt(index);
                        repo.SaveData(listData);

                        AuthManager auth = new AuthManager();
                        auth.SaveLog($"Hapus Data Password: {namaAplikasi}", "Success");

                        LoadDataToGrid();

                        // Post-kondisi mengembalikan message 
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // Log jika terjadi error saat proses hapus
                        AuthManager auth = new AuthManager();
                        auth.SaveLog($"Gagal Hapus Data: {namaAplikasi}", "Error");

                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pre kondisi harus ada data yg dipilih sebelum edit(sama seperti yang hapus tadi)
            Debug.Assert(dataGridView1.CurrentRow != null, "Kontrak Gagal: Tidak bisa edit jika baris kosong.");
            if (dataGridView1.CurrentRow != null)
            {
                PasswordModel dataTerpilih = (PasswordModel)dataGridView1.CurrentRow.DataBoundItem;
                int index = dataGridView1.CurrentRow.Index;

                //invariant
                Debug.Assert(dataTerpilih != null, "Kontrak Gagal: Objek data yang akan diedit tidak ditemukan.");

                FormInputData formEdit = new FormInputData(dataTerpilih, index);
                formEdit.ShowDialog();

                //post kondisi
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
                // Mengambil data utuh dari baris yang diklik
                var data = (PasswordModel)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                // Membuka form detail dan mengirim data (Aplikasi, User, Password Enkripsi)
                FormDetailPassword formDetail = new FormDetailPassword(data.NamaAplikasi, data.Username, data.Password);
                formDetail.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)

        {

        }
    }
}