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
    public partial class FormForgotPassword : Form
    {
        private AuthManager auth = AuthManager.GetInstance();
        public FormForgotPassword()
        {
            InitializeComponent();
        }
        private void FormForgotPassword_Load(object sender, EventArgs e)
        {
            // Ambil pertanyaan dari file JSON secara otomatis saat form terbuka
            string question = auth.GetSecurityQuestion();
            if (!string.IsNullOrEmpty(question))
            {
                lblQuestion.Text = question;
            }
            else
            {
                lblQuestion.Text = "Pertanyaan keamanan belum diatur pada sistem.";
                btnVerify.Enabled = false;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            bool isValid = auth.ValidateRecoveryAnswer(txtAnswer.Text);
            if (isValid)
            {
                MessageBox.Show("Verifikasi Sukses! Status aplikasi dikembalikan ke mode Setup. Silakan buat password baru Anda.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Tutup form lupa password
            }
            else
            {
                MessageBox.Show("Jawaban Anda salah! Silakan coba lagi.", "Verifikasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
