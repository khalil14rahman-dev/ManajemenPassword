using System;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormSetting : Form
    {
        private AuthManager auth = AuthManager.GetInstance();

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

            if (string.IsNullOrWhiteSpace(passLama) || string.IsNullOrWhiteSpace(passBaru) || string.IsNullOrWhiteSpace(konfirmasi))
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (passBaru != konfirmasi)
            {
                MessageBox.Show("Konfirmasi password baru tidak cocok!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PasswordRequestDto authRequest = new PasswordRequestDto { Password = passLama };

            bool sukses = auth.UpdateState(authRequest);

            if (sukses)
            {
                PasswordRequestDto validasiPassBaru = new PasswordRequestDto { Password = passBaru };

                if (!validasiPassBaru.IsValid())
                {
                    MessageBox.Show("Password baru wajib minimal 8 karakter demi keamanan!", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    auth.ChangePassword(passBaru);

                    auth.SaveLog("Ganti Master Password", "Success");
                    MessageBox.Show("Master Password berhasil diperbarui! Aplikasi akan memuat ulang.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Application.Restart();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Peringatan Keamanan Backend", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                auth.SaveLog("Gagal Ganti Master Password (Password Lama Salah)", "Failed");
                MessageBox.Show("Password lama yang Anda masukkan salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            txtPassLama.UseSystemPasswordChar = true;
            txtPassBaru.UseSystemPasswordChar = true;
            txtKonfirmasi.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassLama.UseSystemPasswordChar = !chkShowLama.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            txtPassBaru.UseSystemPasswordChar = !chkShowBaru.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            txtKonfirmasi.UseSystemPasswordChar = !chkShowKonf.Checked;
        }

        private void txtPassLama_TextChanged(object sender, EventArgs e) { }
    }
}