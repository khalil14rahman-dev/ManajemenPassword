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
        DataRepository<PasswordModel> repo = new DataRepository<PasswordModel>("data_password.json");
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
            // TABLE-DRIVEN
            string[] karakterTabel = {
                "ABCDEFGHJKLMNPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "123456789",
                "!@#$%^&*"
            };

            Random rand = new Random();
            string passwordBaru = "";

            for (int i = 0; i < karakterTabel.Length; i++)
            {
                string barisKarakter = karakterTabel[i];
                for (int j = 0; j < 2; j++)
                {
                    passwordBaru += barisKarakter[rand.Next(barisKarakter.Length)];
                }
            }

            textPassword.Text = passwordBaru;
        }

        private void btnSimpanFormInput_Click(object sender, EventArgs e)
        {
            // defensive dan dbc apabila ada inputan yang kosong
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

                //invariant agar list data tidak null
                Debug.Assert(listData != null, "Kontrak Invariant: listData tidak boleh null setelah loading.");

                string passwordAman = SecurityService.Encrypt(textPassword.Text);

                // Masukkan password yang sudah di-Encrypt ke model
                PasswordModel dataInput = new PasswordModel(txtAplikasi.Text, txtUsername.Text, passwordAman);

                AuthManager auth = new AuthManager();

                if (indexEdit == -1)
                {
                    listData.Add(dataInput);
                    repo.SaveData(listData);

                    auth.SaveLog($"Tambah Data: {txtAplikasi.Text}", "Success");
                }
                else
                {
                    //pre kondisi agar logic index edit valid sebelum melakukan perubahhan pada list
                    Debug.Assert(indexEdit >= 0 && indexEdit < listData.Count, "Kontrak: Indeks edit di luar jangkauan.");

                    if (indexEdit >= 0 && indexEdit < listData.Count)
                    {
                        listData[indexEdit] = dataInput;
                        repo.SaveData(listData);

                        auth.SaveLog($"Update Data: {txtAplikasi.Text}", "Success");
                    }
                }

                repo.SaveData(listData);

                MessageBox.Show("Data berhasil dienkripsi dan disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                //buat kalo runtime eror
                AuthManager auth = new AuthManager();
                auth.SaveLog("Tambah Data Password", "Success");
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