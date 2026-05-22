using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class Form1 : Form
    {
        AuthManager auth = new AuthManager();

        public Form1()
        {
            {
                InitializeComponent(); 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private async void btnAction_Click(object sender, EventArgs e)
        {
            string input = txtMasterPassword.Text;


            // DEFENSIVE: Cek apakah sistem sedang terkunci
            if (auth.IsLockedOut())
            {
                MessageBox.Show("Terlalu banyak percobaan! Silakan tunggu 10 detik.", "Sistem Terkunci", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // IMPLEMENTASI DTO: Bungkus string input dari TextBox ke dalam kontainer DTO
            PasswordRequestDto authRequest = new PasswordRequestDto { Password = txtMasterPassword.Text };

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
                    MessageBox.Show("Master Password Berhasil Dibuat! Silakan login ulang.", "Sukses");
                    txtMasterPassword.Clear();
                    RefreshUI(); // Update tampilan ke mode Login
                }
                else if (auth.CurrentState == AppState.DASHBOARD)
                {
                    MessageBox.Show("Login Berhasil! Selamat Datang.", "Informasi");

                    //  KODE PENGHUBUNG KE DASHBOARD 
                    FormDashboard dashboard = new FormDashboard(); // Memanggil Form
                    dashboard.Show(); // Menampilkan Dashboard

                    this.Hide(); // Menyembunyikan Form Login (Form1)
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
            }
            else if (auth.CurrentState == AppState.LOGIN)
            {
                lblStatus.Text = "Masukkan Master Password Anda:";
                btnAction.Text = "Masuk";
                lnkLupaPassword.Visible = true;  // Tampilkan link hanya pada menu Login
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
            if (chkShowPassword.Checked)
            {
                // Jika dicentang, karakter password ditampilkan (normal)
                txtMasterPassword.UseSystemPasswordChar = false;
            }
            else
            {
                // Jika tidak dicentang, karakter disembunyikan (bulatan/bintang)
                txtMasterPassword.UseSystemPasswordChar = true;
            }
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
                bool isReset = auth.ResetMasterKey();
                if (isReset)
                {
                    MessageBox.Show("Master Password berhasil direset! Silakan buat password baru Anda.", "Sukses");
                    txtMasterPassword.Clear();
                    RefreshUI(); // Mengubah tampilan Form kembali ke mode SETUP awal
                }
            }
        }
    }
}