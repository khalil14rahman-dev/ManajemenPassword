using System;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormSetting : Form
    {
        // [CLEAN CODE] - Menerapkan standar penamaan variabel global & readonly
        private readonly AuthManager _auth = new AuthManager();

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

            // [CLEAN CODE: DEFENSIVE PROGRAMMING]
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

            // Memanggil logika dari kelas AuthManager (Penerapan Single Responsibility)
            bool sukses = _auth.UpdateState(passLama);

            if (sukses)
            {
                _auth.ChangePassword(passBaru);
                MessageBox.Show("Master Password berhasil diperbarui! Silakan Login ulang.");

                Application.Restart();
            }
            else
            {
                MessageBox.Show("Password lama salah!");
            }
        }

        // [CLEAN CODE: DEFENSIVE UI] - Inversi nilai boolean untuk fitur Show/Hide
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
    }
}