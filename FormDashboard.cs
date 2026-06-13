using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormDashboard : Form
    {
        DataRepository<PasswordModel> repo = DataRepository<PasswordModel>.GetInstance("data_password.json");
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

            //buat memanggil observer yang ada di datarepo,
            repo.OnDataChanged += LoadDataToGrid;

            LoadDataToGrid();
            dataGridView1.RowHeadersWidth = 50;
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
                throw new InvalidOperationException("DbC Violation [Pre-condition]: Tidak ada baris yang dipilih atau CurrentRow bernilai null!");
            }

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

                        int jumlahBarisAwal = dataGridView1.Rows.Count;

                        Debug.Assert(index >= 0 && index < listData.Count, "Kontrak Gagal: Indeks di luar jangkauan list data.");

                        if (index < 0 || index >= listData.Count)
                        {
                            throw new ArgumentOutOfRangeException(nameof(index), "DbC Violation [Invariant]: Indeks baris di UI tidak sinkron dengan jumlah data di database JSON!");
                        }

                        listData.RemoveAt(index);
                        repo.SaveData(listData);

                        AuthManager auth = new AuthManager();
                        auth.SaveLog($"Hapus Data Password: {namaAplikasi}", "Success");


                        if (dataGridView1.Rows.Count != (jumlahBarisAwal - 1))
                        {
                            throw new InvalidOperationException("DbC Violation [Post-condition]: Fungsi LoadDataToGrid() gagal memperbarui tampilan tabel secara sinkron!");
                        }

                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        AuthManager auth = new AuthManager();
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

                FormDetailPassword formDetail = new FormDetailPassword(data.NamaAplikasi, data.Username, data.Password);
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

            List<int> indeksYangDihapus = new List<int>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var cellValue = dataGridView1.Rows[i].Cells["chkDelete"].Value;

                bool isChecked = cellValue != null && Convert.ToBoolean(cellValue);

                if (isChecked)
                {
                    indeksYangDihapus.Add(i); 
                }
            }

            if (indeksYangDihapus.Count == 0)
            {
                MessageBox.Show("Silakan centang data yang ingin dihapus terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Apakah Anda yakin ingin menghapus {indeksYangDihapus.Count} data yang dicentang?",
                "Konfirmasi Hapus Massal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    List<PasswordModel> listData = repo.LoadData();

                    indeksYangDihapus.Reverse();

                    foreach (int index in indeksYangDihapus)
                    {
                        if (index >= 0 && index < listData.Count)
                        {
                            listData.RemoveAt(index);
                        }
                    }

                    repo.SaveData(listData);

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
    }
}