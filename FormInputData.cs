using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormInputData : Form
    {
        //generic
        DataRepository<PasswordModel> repo = DataRepository<PasswordModel>.GetInstance("data_password.json");
        private int indexEdit = -1;

        public FormInputData()
        {
            InitializeComponent();
        }

        public FormInputData(PasswordModel data, int index)
        {
            InitializeComponent();
            this.indexEdit = index;

            txtAplikasi.Text = data.NamaAplikasi;
            txtUsername.Text = data.Username;

            textPassword.Text = SecurityService.Decrypt(data.Password);

            btnSimpanFormInput.Text = "Update Data";
        }

        private void btnBatalFormInput_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            // 1. TABLE-DRIVEN
            string[] karakterTabel = {
        "ABCDEFGHJKLMNPQRSTUVWXYZ",
        "abcdefghijkmnopqrstuvwxyz",
        "123456789",
        "!@#$%^&*"
    };

            List<char> passwordCharList = new List<char>();

            using (var crypto = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                for (int i = 0; i < karakterTabel.Length; i++)
                {
                    string barisKarakter = karakterTabel[i];
                    for (int j = 0; j < 3; j++)
                    {
                        byte[] buffer = new byte[1];
                        crypto.GetBytes(buffer);

                        int indeksAcak = buffer[0] % barisKarakter.Length;
                        passwordCharList.Add(barisKarakter[indeksAcak]);
                    }
                }

                byte[] shuffleBuffer = new byte[passwordCharList.Count];
                crypto.GetBytes(shuffleBuffer);

                for (int i = passwordCharList.Count - 1; i > 0; i--)
                {
                    int j = shuffleBuffer[i] % (i + 1);
                    char temp = passwordCharList[i];
                    passwordCharList[i] = passwordCharList[j];
                    passwordCharList[j] = temp;
                }
            }

            textPassword.Text = new string(passwordCharList.ToArray());
        }

        private void btnSimpanFormInput_Click(object sender, EventArgs e)
        {
            // validasi input kosong
            if (string.IsNullOrWhiteSpace(txtAplikasi.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(textPassword.Text))
            {
                MessageBox.Show("Semua kolom harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                List<PasswordModel> listData = repo.LoadData();

                // DbC Invariant
                Debug.Assert(listData != null, "Kontrak Invariant: listData tidak boleh null.");
                if (listData == null)
                    throw new InvalidOperationException("DbC Violation [Invariant]: listData bernilai null!");

                // clean code Jika mode edit, validasi indeks harus benar dulu di awal sebelum lanjut
                if (indexEdit != -1 && (indexEdit < 0 || indexEdit >= listData.Count))
                {
                    throw new ArgumentOutOfRangeException(nameof(indexEdit), "DbC Violation [Pre-condition]: Indeks data di luar jangkauan!");
                }

                string passwordAman = SecurityService.Encrypt(textPassword.Text);
                PasswordModel dataInput = new PasswordModel(txtAplikasi.Text, txtUsername.Text, passwordAman);
                AuthManager auth = new AuthManager();

                if (indexEdit == -1)
                {
                    // Mode Tambah Data Baru
                    listData.Add(dataInput);
                    auth.SaveLog($"Tambah Data: {txtAplikasi.Text}", "Success");
                }
                else
                {
                    // Mode Update Data 
                    listData[indexEdit] = dataInput;
                    auth.SaveLog($"Update Data: {txtAplikasi.Text}", "Success");
                }

                repo.SaveData(listData);

                MessageBox.Show("Data berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                AuthManager auth = new AuthManager();
                auth.SaveLog("Proses Data Password", "Failed");
                MessageBox.Show("Gagal memproses data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public static (string Status, Color Warna, int Score) CalculatePasswordStrength(string pass)
        {
            if (string.IsNullOrEmpty(pass))
            {
                return ("Kekuatan: -", Color.Gray, 0);
            }

            var passwordRules = new List<Func<string, bool>>
            {
                p => p.Length >= 8,
                p => p.Any(char.IsUpper) && p.Any(char.IsLower),
                p => p.Any(char.IsDigit),
                p => p.Any(ch => !char.IsLetterOrDigit(ch))
            };

            int score = passwordRules.Count(rule => rule(pass));

            var strengthTable = new Dictionary<int, (string Status, Color Warna)>
            {
                { 0, ("Kekuatan: Sangat Lemah", Color.Red) },
                { 1, ("Kekuatan: Lemah", Color.Red) },
                { 2, ("Kekuatan: Sedang", Color.Orange) },
                { 3, ("Kekuatan: Kuat", Color.LightGreen) },
                { 4, ("Kekuatan: Sangat Kuat", Color.Green) }
            };

            Debug.Assert(strengthTable.Count == 5, "Invariant Failed: Table size must be 5.");

            if (strengthTable.TryGetValue(score, out var result))
            {
                // POSTCONDITIONS
                Debug.Assert(score >= 0 && score <= passwordRules.Count, "Postcondition Failed: Score out of bounds.");
                return (result.Status, result.Warna, score);
            }

            return ("Kekuatan: -", Color.Gray, 0);
        }

        private void textPassword_TextChanged_1(object sender, EventArgs e)
        {
            // PRECONDITION
            Debug.Assert(sender != null, "Precondition Failed: Sender cannot be null.");

            var result = CalculatePasswordStrength(textPassword.Text);

            lblstrength.Text = result.Status;
            lblstrength.ForeColor = result.Warna;
        }
    }
}