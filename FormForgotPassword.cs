using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormForgotPassword : Form
    {
        private AuthManager auth = AuthManager.GetInstance();
        private List<Label> lblQuestionsList = new List<Label>();
        private List<TextBox> txtAnswersList = new List<TextBox>();
        private List<KeyValuePair<int, string>> loadedQuestions = new List<KeyValuePair<int, string>>();

        private TextBox txtNewPassword;
        private bool isVerificationPassed = false; 
        private int targetUserId = -1;

        public FormForgotPassword()
        {
            InitializeComponent();
        }

        private void FormForgotPassword_Load(object sender, EventArgs e)
        {
            btnVerify.Enabled = false;
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            string usernameInput = txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(usernameInput))
            {
                MessageBox.Show("Masukkan username Anda terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            targetUserId = auth.GetUserIdByUsername(usernameInput);
            loadedQuestions = auth.GetAllSecurityQuestions(usernameInput);

            flowLayoutPanelRecovery.Controls.Clear();
            lblQuestionsList.Clear();
            txtAnswersList.Clear();

            if (loadedQuestions.Count > 0)
            {
                foreach (var q in loadedQuestions)
                {
                    Panel rowPanel = new Panel { Width = flowLayoutPanelRecovery.Width - 25, Height = 55 };

                    Label lbl = new Label { Text = q.Value, Width = rowPanel.Width - 10, Location = new System.Drawing.Point(5, 2) };
                    TextBox txt = new TextBox { Width = rowPanel.Width - 10, Location = new System.Drawing.Point(5, 25) };

                    lblQuestionsList.Add(lbl);
                    txtAnswersList.Add(txt);

                    rowPanel.Controls.Add(lbl);
                    rowPanel.Controls.Add(txt);
                    flowLayoutPanelRecovery.Controls.Add(rowPanel);
                }

                btnVerify.Enabled = true;
                flowLayoutPanelRecovery.PerformLayout();
            }
            else
            {
                MessageBox.Show("Username tidak ditemukan atau user belum mengatur pertanyaan keamanan.", "Informasi");
                btnVerify.Enabled = false;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (isVerificationPassed)
            {
                string newPassword = txtNewPassword.Text;
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
                {
                    MessageBox.Show("Password baru wajib diisi dan minimal 8 karakter!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    auth.SetSession(targetUserId, txtUsername.Text.Trim());
                    auth.ChangePassword(newPassword);

                    MessageBox.Show("Master Password Anda berhasil diperbarui! Silakan kembali login.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    auth.ClearSession();
                    auth.ChangeState(new LoginState());
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengubah password: " + ex.Message, "Error");
                }
                return;
            }

            List<KeyValuePair<int, string>> userAnswers = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < loadedQuestions.Count; i++)
            {
                userAnswers.Add(new KeyValuePair<int, string>(loadedQuestions[i].Key, txtAnswersList[i].Text.Trim()));
            }

            bool isAllCorrect = auth.ValidateAllRecoveryAnswers(targetUserId, userAnswers);

            if (isAllCorrect)
            {
                MessageBox.Show("Semua verifikasi sukses! Silakan masukkan Master Password baru Anda pada kolom yang tersedia.", "Verifikasi Berhasil");
                isVerificationPassed = true;

                txtUsername.Enabled = false;
                btnCari.Enabled = false;

                flowLayoutPanelRecovery.Controls.Clear();

                Panel passPanel = new Panel { Width = flowLayoutPanelRecovery.Width - 25, Height = 60 };
                Label lblNewPass = new Label { Text = "Masukkan Master Password Baru:", Width = passPanel.Width - 10, Location = new System.Drawing.Point(5, 5) };
                txtNewPassword = new TextBox { Width = passPanel.Width - 10, Location = new System.Drawing.Point(5, 28), UseSystemPasswordChar = true };

                passPanel.Controls.Add(lblNewPass);
                passPanel.Controls.Add(txtNewPassword);
                flowLayoutPanelRecovery.Controls.Add(passPanel);

                btnVerify.Text = "Simpan Password Baru";
                flowLayoutPanelRecovery.PerformLayout();
            }
            else
            {
                MessageBox.Show("Ada jawaban Anda yang salah! Silakan periksa kembali.", "Verifikasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}