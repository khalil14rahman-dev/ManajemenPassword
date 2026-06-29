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
        DataRepository repo = DataRepository.GetInstance();

        private int currentIdPassword = -1;

        public FormInputData()
        {
            InitializeComponent();
            LoadCategoryComboBox();
        }

        public FormInputData(PasswordModel data, int index)
        {
            InitializeComponent();
            LoadCategoryComboBox(); 

            this.currentIdPassword = data.IdPassword;
            txtAplikasi.Text = data.NamaAplikasi;
            txtUsername.Text = data.UsernameAkun;
            textPassword.Text = SecurityService.Decrypt(data.Password);

            cmbKategori.SelectedValue = data.IdCategory;

            btnSimpanFormInput.Text = "Update Data";

            textPassword_TextChanged(textPassword, EventArgs.Empty);
        }

        private void btnBatalFormInput_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            textPassword.Text = PasswordModel.GeneratePassword();
        }

        private void LoadCategoryComboBox()
        {
            var listKategori = repo.GetCategories();

            cmbKategori.DataSource = new BindingSource(listKategori, null);
            cmbKategori.DisplayMember = "Value"; 
            cmbKategori.ValueMember = "Key";     
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
                string passwordAman = SecurityService.Encrypt(textPassword.Text);

                PasswordModel dataInput = new PasswordModel();
                dataInput.NamaAplikasi = txtAplikasi.Text;
                dataInput.UsernameAkun = txtUsername.Text;
                dataInput.Password = passwordAman;

                dataInput.IdCategory = (int)cmbKategori.SelectedValue;

                if (currentIdPassword == -1)
                {
                    dataInput.IdPassword = 0;
                }
                else
                {
                    dataInput.IdPassword = currentIdPassword;
                }

                repo.SaveData(dataInput);

                AuthManager auth = AuthManager.GetInstance();
                auth.SaveLog($"Simpan Data Aplikasi: {txtAplikasi.Text}", "Success");

                MessageBox.Show("Data berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            StrengthFactory factory = new PasswordStrengthFactory();
            return factory.CreateStrengthResult(score);
        }
    }
}