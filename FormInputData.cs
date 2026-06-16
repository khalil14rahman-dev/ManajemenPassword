using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

                repo.SaveData(listData);

                MessageBox.Show("Data berhasil dienkripsi dan disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memproses data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AuthManager auth = AuthManager.GetInstance();
            auth.SaveLog("Tambah Data Password", "Success");
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textPassword.Text))
            {
                lblstrength.Text = "Kekuatan: -";
                lblstrength.ForeColor = Color.Gray;
            }
            else if (textPassword.Text.Length < 8)
            {
                lblstrength.Text = "Kekuatan: Lemah (Min. 8 Karakter)";
                lblstrength.ForeColor = Color.Red;
            }
            else
            {
                lblstrength.Text = "Kekuatan: Kuat";
                lblstrength.ForeColor = Color.Green;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblstrength_Click(object sender, EventArgs e)
        {
            string pass = textPassword.Text;

            if (string.IsNullOrEmpty(pass))
            {
                lblstrength.Text = "Kekuatan : -";
                lblstrength.ForeColor = Color.Gray;
            }
            else if (pass.Length < 8)
            {
                lblstrength.Text = "Kekuatan : Lemah (Terlalu Pendek)";
                lblstrength.ForeColor = Color.Red;
            }
            else
            {
                lblstrength.Text = "Kekuatan : Sangat Kuat";
                lblstrength.ForeColor = Color.Green;
            }
        }

        private void textPassword_TextChanged_1(object sender, EventArgs e)
        {
            string pass = textPassword.Text;
            int len = pass.Length;

            var strengthTable = new Dictionary<int, (string Status, Color Warna)>
    {
        { 0, ("Kekuatan: -", Color.Gray) },
        { 1, ("Kekuatan: Lemah", Color.Red) },
        { 8, ("Kekuatan: Sedang", Color.Orange) },
        { 12, ("Kekuatan: Sangat Kuat", Color.Green) }
    };

            string statusBaru = "Kekuatan: -";
            Color warnaBaru = Color.Gray;

            foreach (var rule in strengthTable)
            {
                if (len >= rule.Key)
                {
                    statusBaru = rule.Value.Status;
                    warnaBaru = rule.Value.Warna;
                }
            }

            lblstrength.Text = statusBaru;
            lblstrength.ForeColor = warnaBaru;
        }

        private void FormInputData_Load(object sender, EventArgs e)
        {

        }
    }
}