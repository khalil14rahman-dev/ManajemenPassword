using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormInputData : Form
    {
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
            textPassword_TextChanged(textPassword, EventArgs.Empty);
        }

        private void btnBatalFormInput_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
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
                string passwordAman = SecurityService.Encrypt(textPassword.Text);
                PasswordModel dataInput = new PasswordModel(txtAplikasi.Text, txtUsername.Text, passwordAman);

                if (indexEdit == -1)
                {
                    listData.Add(dataInput);
                }
                else
                {
                    if (indexEdit >= 0 && indexEdit < listData.Count)
                    {
                        listData[indexEdit] = dataInput;
                    }
                }

                // Saat baris ini jalan, Event Observer akan otomatis terpanggil, memanggil yang ada di datarepo
                //disini juga ada clean code DRY
                repo.SaveData(listData);

                MessageBox.Show("Data berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AuthManager auth = AuthManager.GetInstance();
            auth.SaveLog("Tambah/Update Data Password", "Success");
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            Debug.Assert(sender != null, "Precondition gagal: pengirim tidak boleh null.");

            StrengthResult result = CalculatePasswordStrength(textPassword.Text);

            lblstrength.Text = result.Status;
            lblstrength.ForeColor = result.Warna;
        }

        public static StrengthResult CalculatePasswordStrength(string pass)
        {
            if (string.IsNullOrEmpty(pass)) return new StrengthResult("Kekuatan: -", Color.Gray, 0);

            var passwordRules = new List<Func<string, bool>>
{
    p => p.Length >= 8,
    p => p.Any(char.IsUpper) && p.Any(char.IsLower),
    p => p.Any(char.IsDigit),
    p => p.Any(ch => !char.IsLetterOrDigit(ch))
};

            int score = passwordRules.Count(rule => rule(pass));

            // Melempar skor ke Factory Design Pattern kelompokmu
            StrengthFactory factory = new PasswordStrengthFactory();
            return factory.CreateStrengthResult(score);
        }

    }
}
   