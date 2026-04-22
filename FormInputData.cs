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
    public partial class FormInputData : Form
    {
        DataRepository<PasswordModel> repo = new DataRepository<PasswordModel>("data_password.json");
        private int indexEdit = -1; // -1 artinya mode TAMBAH, kalau >= 0 artinya mode EDIT

        // Constructor asli (jangan dihapus)
        public FormInputData()
        {
            InitializeComponent();
        }

        // Tambah Constructor baru untuk EDIT
        public FormInputData(PasswordModel data, int index)
        {
            InitializeComponent();
            this.indexEdit = index;

            // Isi textbox dengan data yang mau diedit
            txtAplikasi.Text = data.NamaAplikasi;
            txtUsername.Text = data.Username;
            textPassword.Text = data.Password;

            btnSimpanFormInput.Text = "Update Data"; // Ganti tulisan tombolnya
        }

        private void btnBatalFormInput_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            // TABLE-DRIVEN 
            string[] karakterTabel = {
            "ABCDEFGHJKLMNPQRSTUVWXYZ", // Huruf Besar
            "abcdefghijkmnopqrstuvwxyz", // Huruf Kecil
            "123456789",                 // Angka
            "!@#$%^&*"                   // Simbol
        };

            Random rand = new Random();
            string passwordBaru = "";

            // Logika mengambil 2 karakter dari setiap baris tabel
            for (int i = 0; i < karakterTabel.Length; i++)
            {
                string barisKarakter = karakterTabel[i];
                for (int j = 0; j < 2; j++) // Ambil 2 karakter per kategori
                {
                    passwordBaru += barisKarakter[rand.Next(barisKarakter.Length)];
                }
            }

            // Masukkan ke TextBox Password kamu
            textPassword.Text = passwordBaru;
        }

        private void btnSimpanFormInput_Click(object sender, EventArgs e)
        {
            // --- DEFENSIVE PROGRAMMING ---
            if (string.IsNullOrWhiteSpace(txtAplikasi.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(textPassword.Text))
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Ambil data lama dulu dari JSON
                List<PasswordModel> listData = repo.LoadData();

                // 2. Buat objek data dari inputan TextBox
                PasswordModel dataInput = new PasswordModel(txtAplikasi.Text, txtUsername.Text, textPassword.Text);

                // 3. LOGIKA UPDATE (EDIT) ATAU TAMBAH (ADD)
                // Jika indexEdit adalah -1 berarti data baru, jika >= 0 berarti sedang mengedit
                if (indexEdit == -1)
                {
                    listData.Add(dataInput); // Tambah baru
                }
                else
                {
                    // Pastikan index yang diedit masih valid dalam list
                    if (indexEdit >= 0 && indexEdit < listData.Count)
                    {
                        listData[indexEdit] = dataInput; // Timpa data lama dengan data baru
                    }
                }

                // 4. Simpan kembali List terbaru ke file JSON
                repo.SaveData(listData);

                MessageBox.Show("Data berhasil diproses!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                // --- EXCEPTION HANDLING ---
                MessageBox.Show("Gagal memproses data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
