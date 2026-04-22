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
    public partial class FormSetting : Form
    {
        AuthManager auth = new AuthManager();
        public FormSetting()
        {
            InitializeComponent();
        }

        private void btnBatalFormSetting_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdateFormSetting_Click(object sender, EventArgs e)
        {
            string passLama = txtPassLama.Text;
            string passBaru = txtPassBaru.Text;
            string konfirmasi = txtKonfirmasi.Text;

            // 1. Validasi: Jangan ada yang kosong
            if (string.IsNullOrEmpty(passLama) || string.IsNullOrEmpty(passBaru))
            {
                MessageBox.Show("Semua kolom harus diisi!");
                return;
            }

            // 2. Cek Konfirmasi: Password baru harus sama dengan konfirmasi
            if (passBaru != konfirmasi)
            {
                MessageBox.Show("Konfirmasi password baru tidak cocok!");
                return;
            }

            // 3. Eksekusi Ganti Password (Materi State Transition)
            // Kita asumsikan di AuthManager ada fungsi untuk ganti password
            bool sukses = auth.UpdateState(passLama); // Cek dulu password lamanya bener gak

            if (sukses)
            {
                auth.ChangePassword(passBaru);
                // Di sini kamu perlu koordinasi dengan temanmu 
                // untuk fungsi menyimpan password baru ke file auth
                MessageBox.Show("Master Password berhasil diperbarui! Silakan Login ulang.");

                // Logout otomatis agar user tes password barunya
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Password lama salah!");
            }
        }
    }
}
