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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            AuthManager auth = new AuthManager();
            string inputUser = txtMasterPassword.Text;

            if (string.IsNullOrWhiteSpace(inputUser))
            {
                MessageBox.Show("Password tidak boleh kosong!");
                return;
            }

            if (auth.IsFirstTime())
            {
                // Kondisi: Daftar Baru
                auth.SimpanPasswordBaru(inputUser);
                MessageBox.Show("Master Password Berhasil Dibuat! Silakan login ulang.");
                Application.Restart(); // Kita restart biar state-nya berubah jadi Login
            }
            else
            {
                // Kondisi: Login Biasa
                if (auth.CekPassword(inputUser))
                {
                    MessageBox.Show("Login Berhasil! Selamat Datang.");
                    // Nanti di sini kita buka Dashboard
                }
                else
                {
                    MessageBox.Show("Password Salah!");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AuthManager auth = new AuthManager();
            if (auth.IsFirstTime())
            {
                lblStatus.Text = "Halo! Silakan buat Master Password pertamamu.";
                btnAction.Text = "Buat Password";
            }
            else
            {
                lblStatus.Text = "Masukkan Master Password Anda:";
                btnAction.Text = "Masuk";
            }
        }
    }
}
