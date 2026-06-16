using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class Form1 : Form
    {
        AuthManager auth = AuthManager.GetInstance();

        public Form1()
        {
            InitializeComponent(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshUI();

            // =============== LIVE CONNECTION TEST ===============
            try
            {
                // 1. Ambil instance database helper
                DatabaseConnection db = DatabaseConnection.GetInstance();

                // 2. Coba buka koneksi ke MySQL
                db.OpenConnection();

                // 3. Jika berhasil sampai baris ini tanpa throw error, berarti SUKSES
                MessageBox.Show("Koneksi ke Database MySQL (phpMyAdmin) Berhasil!",
                                "Status Koneksi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // 4. Jika gagal (misal XAMPP mati atau nama DB salah), error ditangkap di sini
                MessageBox.Show(ex.Message,
                                "Koneksi Database Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                // 5. Selalu tutup kembali koneksi setelah dites agar tidak menggantung
                DatabaseConnection.GetInstance().CloseConnection();
            }
        }

        private async void btnAction_Click(object sender, EventArgs e)
        {
            // DEFENSIVE: Cek apakah sistem sedang terkunci
            if (auth.IsLockedOut())
            {
                MessageBox.Show("Terlalu banyak percobaan! Silakan tunggu 10 detik.", "Sistem Terkunci", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // ==================== PERBAIKAN DI SINI ====================
            // Ikat semua data inputan dari Form ke dalam satu paket DTO
            PasswordRequestDto authRequest = new PasswordRequestDto
            {
                Password = txtMasterPassword.Text,
                SecurityQuestion = cmbSecurityQuestion.SelectedItem?.ToString(), // Ambil teks pertanyaan pilihan
                SecurityAnswer = txtSecurityAnswer.Text // Ambil teks jawaban teks
            };

            // Validasi tambahan khusus mode SETUP
            if (auth.CurrentState == AppState.SETUP)
            {
                if (string.IsNullOrWhiteSpace(authRequest.SecurityQuestion) || string.IsNullOrWhiteSpace(authRequest.SecurityAnswer))
                {
                    MessageBox.Show("Pertanyaan dan Jawaban Keamanan wajib dipilih & diisi untuk pemulihan akun!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            // ===========================================================
            // DEFENSIVE VIA DTO: Cek keabsahan data lewat satu gerbang fungsi objek DTO
            if (!authRequest.IsValid())
            {
                MessageBox.Show("Password tidak boleh kosong dan wajib minimal 8 karakter demi keamanan!", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppState stateSebelumnya = auth.CurrentState;

            // Eksekusi transisi Automata dengan melemparkan paket DTO
            bool isSuccess = auth.UpdateState(authRequest);

            if (isSuccess)
            {
                if (stateSebelumnya == AppState.SETUP)
                {
                    MessageBox.Show("Master Password & Kunci Pemulihan Berhasil Dibuat! Silakan login ulang.", "Sukses");
                    txtMasterPassword.Clear();
                    txtSecurityAnswer.Clear(); // Bersihkan field jawaban setelah sukses
                    RefreshUI(); // Update tampilan ke mode Login
                }
                else if (auth.CurrentState == AppState.DASHBOARD)
                {
                    MessageBox.Show("Login Berhasil! Selamat Datang.", "Informasi");

                    FormDashboard dash = new FormDashboard();
                    dash.Show();
                    this.Hide();
                }
            }
            else
            {
                // Jika isSuccess false pada state LOGIN, berarti password salah
                if (auth.CurrentState == AppState.LOGIN)
                {
                    // DEFENSIVE: Cek jika baru saja mencapai batas kesalahan
                    if (auth.IsLockedOut())
                    {
                        btnAction.Enabled = false; // Matikan tombol
                        MessageBox.Show("3 kali salah! Tombol dimatikan selama 10 detik.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        await Task.Delay(10000); // Jeda 10 detik secara asinkron

                        auth.ResetAttempts(); // Reset hitungan di manager
                        btnAction.Enabled = true; // Aktifkan tombol lagi
                        MessageBox.Show("Silakan coba login kembali.", "Informasi");
                    }
                    else
                    {
                        MessageBox.Show("Password Salah!", "Error");
                    }
                }
            }
        }

        // STATE-BASED UI
        private void RefreshUI()
        {
            if (auth.CurrentState == AppState.SETUP)
            {
                lblStatus.Text = "Halo! Silakan buat Master Password pertamamu.";
                btnAction.Text = "Buat Password";
                lnkLupaPassword.Visible = false; // Sembunyikan link jika masih setup awal

                // TAMPILKAN komponen pertanyaan keamanan saat mode Setup
                cmbSecurityQuestion.Visible = true;
                txtSecurityAnswer.Visible = true;
                lblQuestionHint.Visible = true;
                lblAnswerHint.Visible = true;
            }
            else if (auth.CurrentState == AppState.LOGIN)
            {
                lblStatus.Text = "Masukkan Master Password Anda:";
                btnAction.Text = "Masuk";
                lnkLupaPassword.Visible = true;  // Tampilkan link hanya pada menu Login

                // SEMBUNYIKAN komponen pertanyaan keamanan saat mode Login
                cmbSecurityQuestion.Visible = false;
                txtSecurityAnswer.Visible = false;
                lblQuestionHint.Visible = false;
                lblAnswerHint.Visible = false;
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void txtMasterPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // DEFENSIVE & UX: Mengatur visibilitas karakter berdasarkan status CheckBox
            txtMasterPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void lnkLupaPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // DEFENSIVE: Konfirmasi ulang ke user agar tidak tidak sengaja terhapus
            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin mereset Master Password? Semua data password yang tersimpan akan tetap aman, namun Anda harus membuat Master Password baru.",
                "Konfirmasi Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // Buka form pertanyaan keamanan secara dialog modal
                FormForgotPassword forgotForm = new FormForgotPassword();
                forgotForm.ShowDialog();

                // Jika user sukses menjawab, otomatis State berubah menjadi SETUP.
                // Kita cek di sini untuk merefresh tampilan Form1 ke mode pembuatan password baru.
                if (auth.CurrentState == AppState.SETUP)
                {
                    txtMasterPassword.Clear();
                    RefreshUI(); // Form1 otomatis berubah mode UI-nya ke Setup awal
                }
            }
        }
    }
}