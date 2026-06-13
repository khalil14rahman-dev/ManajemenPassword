using System;
using System.Collections.Generic;
using System.Drawing;
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
            textPassword.Text = SecurityService.Decrypt(data.Password)
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

            AuthManager auth = new AuthManager();
            auth.SaveLog("Tambah/Update Data Password", "Success");
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
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
    }
}