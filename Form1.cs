using System;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class Form1 : Form
    {
        // Instansiasi AuthManager 
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

        private void btnAction_Click(object sender, EventArgs e)
        {
            string input = txtMasterPassword.Text;

            // STRATEGI DEFENSIVE PROGRAMMING 
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (auth.CurrentState == AppState.SETUP && input.Length < 8)
            {
                // KONDISI SPESIFIK: Memaksa standar keamanan saat pembuatan Master Password
                MessageBox.Show("Master Password terlalu lemah! Gunakan minimal 8 karakter demi keamanan.", "Peringatan Keamanan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Simpan state saat ini sebelum update untuk pengecekan notifikasi
            AppState stateSebelumnya = auth.CurrentState;

            // Eksekusi transisi di Automata
            bool isSuccess = auth.UpdateState(input);

            // LOGIKA RESPON BERDASARKAN HASIL TRANSISI 
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
                    MessageBox.Show("Password Salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // STRATEGI STATE-BASED UI
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
    }
}