using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormDashboard : Form
    {
        DataRepository repo = DataRepository.GetInstance();
        private AuthManager auth = AuthManager.GetInstance();

        public FormDashboard()
        {
            InitializeComponent();

            repo.OnDataChanged += LoadDataToGrid;

            LoadDataToGrid();
            dataGridView1.RowHeadersWidth = 50;
        }

        public void LoadDataToGrid()
        {
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
                dataGridView1.Columns["colUsername"].DataPropertyName = "UsernameAkun";
            }

            if (dataGridView1.Columns.Contains("colPassword"))
            {
                dataGridView1.Columns["colPassword"].DataPropertyName = "Password";
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            FormInputData formInput = new FormInputData();
            formInput.ShowDialog();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FormSetting formSet = new FormSetting();
            formSet.ShowDialog();
        }

        private void hapusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(dataGridView1.CurrentRow != null, "Kontrak Gagal: CurrentRow tidak boleh null saat menghapus.");

            if (dataGridView1.CurrentRow == null)
            {
                throw new InvalidOperationException("DbC Violation [Pre-condition]: Tidak ada baris yang dipilih!");
            }

            PasswordModel dataTerpilih = (PasswordModel)dataGridView1.CurrentRow.DataBoundItem;

            if (dataTerpilih != null)
            {
                string namaAplikasi = dataTerpilih.NamaAplikasi;

                var confirm = MessageBox.Show($"Apakah yakin ingin menghapus akun {namaAplikasi}?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        repo.SoftDeleteData(dataTerpilih.IdPassword);

                        auth.SaveLog($"Hapus Data Password: {namaAplikasi}", "Success");
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        auth.SaveLog($"Gagal Hapus Data: {namaAplikasi}", "Error");
                        MessageBox.Show("Gagal menghapus data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(dataGridView1.CurrentRow != null, "Kontrak Gagal: Tidak bisa edit jika baris kosong.");
            if (dataGridView1.CurrentRow != null)
            {
                PasswordModel dataTerpilih = (PasswordModel)dataGridView1.CurrentRow.DataBoundItem;
                int index = dataGridView1.CurrentRow.Index;

                Debug.Assert(dataTerpilih != null, "Kontrak Gagal: Objek data yang akan diedit tidak ditemukan.");

                FormInputData formEdit = new FormInputData(dataTerpilih, index);
                formEdit.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormLog logWindow = new FormLog();
            logWindow.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = (PasswordModel)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                FormDetailPassword formDetail = new FormDetailPassword(data.NamaAplikasi, data.UsernameAkun, data.Password);
                formDetail.ShowDialog();
            }
        }

        private void btnHapusTerpilih_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data di dalam tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<PasswordModel> dataDihapusMassal = new List<PasswordModel>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var cellValue = dataGridView1.Rows[i].Cells["chkDelete"].Value;
                bool isChecked = cellValue != null && Convert.ToBoolean(cellValue);

                if (isChecked)
                {
                    var data = (PasswordModel)dataGridView1.Rows[i].DataBoundItem;
                    if (data != null) dataDihapusMassal.Add(data);
                }
            }

            if (dataDihapusMassal.Count == 0)
            {
                MessageBox.Show("Silakan centang data yang ingin dihapus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Apakah Anda yakin ingin menghapus {dataDihapusMassal.Count} data yang dicentang?",
                "Konfirmasi Hapus Massal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    foreach (var item in dataDihapusMassal)
                    {
                        repo.SoftDeleteData(item.IdPassword);
                    }

                    MessageBox.Show("Semua data terpilih berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal melakukan hapus massal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string nomorUrut = (e.RowIndex + 1).ToString();
            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            System.Drawing.Rectangle headerBounds = new System.Drawing.Rectangle(
                e.RowBounds.Left, e.RowBounds.Top,
                dataGridView1.RowHeadersWidth, e.RowBounds.Height);

            e.Graphics.DrawString(nomorUrut, this.Font,
                System.Drawing.SystemBrushes.ControlText,
                headerBounds, centerFormat);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            auth.Logout();
            MessageBox.Show("Anda telah berhasil logout.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }
    }
}