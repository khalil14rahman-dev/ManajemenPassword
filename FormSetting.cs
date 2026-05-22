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

            if (string.IsNullOrEmpty(passLama) || string.IsNullOrEmpty(passBaru))
            {
                MessageBox.Show("Semua kolom harus diisi!");
                return;
            }

            if (passBaru != konfirmasi)
            {
                MessageBox.Show("Konfirmasi password baru tidak cocok!");
                return;
            }

            // PERBAIKAN DTO: Bungkus passLama ke dalam DTO sebelum dikirim ke AuthManager
            PasswordRequestDto authRequest = new PasswordRequestDto { Password = passLama };

            // Eksekusi UpdateState menggunakan paket DTO
            bool sukses = auth.UpdateState(authRequest);

            if (sukses)
            {
                // (Opsional/Defensive): Kamu juga bisa membungkus passBaru ke DTO jika ingin memvalidasi panjang 8 karakternya
                PasswordRequestDto validasiPassBaru = new PasswordRequestDto { Password = passBaru };

                if (!validasiPassBaru.IsValid())
                {
                    MessageBox.Show("Password baru harus minimal 8 karakter!", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                auth.ChangePassword(passBaru);
                MessageBox.Show("Master Password berhasil diperbarui! Silakan Login ulang.");

                Application.Restart();
            }
            else
            {
                MessageBox.Show("Password lama salah!");
            }
        }

        private void txtPassLama_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormSetting_Load(object sender, EventArgs e)
        {

        }
    }
}
