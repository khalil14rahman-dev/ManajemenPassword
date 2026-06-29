using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class Form1 : Form
    {
        private AuthManager auth = AuthManager.GetInstance();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMasterPassword.UseSystemPasswordChar = true;

            try
            {
                DatabaseConnection db = DatabaseConnection.GetInstance();
                db.OpenConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Database Gagal: " + ex.Message,
                                "Koneksi Database Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                DatabaseConnection.GetInstance().CloseConnection();
            }
        }

        private async void btnAction_Click(object sender, EventArgs e)
        {
            if (auth.IsLockedOut())
            {
                MessageBox.Show("Terlalu banyak percobaan! Silakan tunggu 10 detik.", "Sistem Terkunci", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string usernameInput = txtUsername.Text.Trim();
            string passwordInput = txtMasterPassword.Text;

            if (string.IsNullOrWhiteSpace(usernameInput) || string.IsNullOrWhiteSpace(passwordInput))
            {
                MessageBox.Show("Username dan Password wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PasswordRequestDto authRequest = new PasswordRequestDto
            {
                Username = usernameInput,
                Password = passwordInput
            };

            bool isSuccess = auth.UpdateState(authRequest);

            if (isSuccess)
            {
                MessageBox.Show($"Login Berhasil! Selamat Datang, {auth.CurrentUsername}.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormDashboard dash = new FormDashboard();
                dash.Show();
                this.Hide(); 
            }
            else
            {
                if (auth.IsLockedOut())
                {
                    btnAction.Enabled = false;
                    MessageBox.Show("3 kali salah! Tombol masuk dimatikan selama 10 detik.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    await Task.Delay(10000);

                    auth.ResetAttempts();
                    btnAction.Enabled = true;
                    MessageBox.Show("Silakan coba login kembali.", "Informasi");
                }
                else
                {
                    MessageBox.Show("Username atau Password Salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtMasterPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void lnkLupaPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin mereset Master Password? Semua data password yang tersimpan akan tetap aman, namun Anda harus memverifikasi pertanyaan keamanan Anda.",
                "Konfirmasi Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                FormForgotPassword forgotForm = new FormForgotPassword();
                forgotForm.ShowDialog();
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRegister registerForm = new FormRegister();
            registerForm.ShowDialog();
        }
    }
}