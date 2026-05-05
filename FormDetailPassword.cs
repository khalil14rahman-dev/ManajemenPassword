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
    public partial class FormDetailPassword : Form
    {
        // VERSI PERBAIKAN: Menambahkan parameter di dalam kurung Constructor
        public FormDetailPassword(string namaApp, string username, string passwordEnkripsi)
        {
            InitializeComponent();

            // Sekarang variabel ini sudah dikenali karena ada di parameter atas
            txtNamaApp.Text = namaApp;
            txtUsername.Text = username;

            // Pastikan SecurityService.Decrypt sudah tersedia di project kamu
            txtPassword.Text = SecurityService.Decrypt(passwordEnkripsi);

            // Opsional: Sembunyikan password saat pertama buka (pakai sensor)
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // Menutup halaman detail
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Bisa dikosongkan jika tidak dipakai
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Bisa dikosongkan jika tidak dipakai
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}