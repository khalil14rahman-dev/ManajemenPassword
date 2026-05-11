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

            // DEFENSIVE PROGRAMMING 
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (auth.CurrentState == AppState.SETUP && input.Length < 8)
            {
                MessageBox.Show("Master Password terlalu lemah! Gunakan minimal 8 karakter demi keamanan.", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simpan state saat ini sebelum update untuk pengecekan notifikasi
            AppState stateSebelumnya = auth.CurrentState;

            // Eksekusi transisi di Automata
            bool isSuccess = auth.UpdateState(input);

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
            }
            else if (auth.CurrentState == AppState.LOGIN)
            {
                lblStatus.Text = "Masukkan Master Password Anda:";
                btnAction.Text = "Masuk";
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
    }
}