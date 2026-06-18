using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project_KPL_ManajemenPassword
{
    public partial class FormRegister : Form
    {
        private List<ComboBox> cmbQuestionsList = new List<ComboBox>();
        private List<TextBox> txtAnswersList = new List<TextBox>();

        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;

            TambahBarisPertanyaanDinamis();
        }

        private void btnTambahPertanyaan_Click(object sender, EventArgs e)
        {
            TambahBarisPertanyaanDinamis();
        }

        private void TambahBarisPertanyaanDinamis()
        {
            Panel rowPanel = new Panel();
            rowPanel.Width = flowLayoutPanel1.Width - 25;
            rowPanel.Height = 35;

            ComboBox cmb = new ComboBox();
            cmb.Width = 250;
            cmb.Location = new System.Drawing.Point(5, 5);

            cmb.DropDownStyle = ComboBoxStyle.DropDown;

            TextBox txt = new TextBox();
            txt.Width = 200;
            txt.Location = new System.Drawing.Point(265, 5);

            IsiDataPertanyaanKeComboBox(cmb);

            cmbQuestionsList.Add(cmb);
            txtAnswersList.Add(txt);
            rowPanel.Controls.Add(cmb);
            rowPanel.Controls.Add(txt);
            flowLayoutPanel1.Controls.Add(rowPanel);
        }

        private void IsiDataPertanyaanKeComboBox(ComboBox cmb)
        {
            DatabaseConnection db = DatabaseConnection.GetInstance();
            try
            {
                db.OpenConnection();
                string query = "SELECT id_question, text_question FROM security_questions";
                using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        var data = new List<KeyValuePair<int, string>>();
                        while (reader.Read())
                        {
                            data.Add(new KeyValuePair<int, string>(
                                Convert.ToInt32(reader["id_question"]),
                                reader["text_question"].ToString()
                            ));
                        }

                        cmb.DataSource = null;
                        cmb.DataSource = new BindingSource(data, null);
                        cmb.DisplayMember = "Value";
                        cmb.ValueMember = "Key";

                        cmb.SelectedIndex = -1;
                        cmb.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mengambil data master pertanyaan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void btnDaftar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username dan Password wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PasswordRequestDto registerDto = new PasswordRequestDto
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text
            };

            AuthManager auth = AuthManager.GetInstance();

            for (int i = 0; i < cmbQuestionsList.Count; i++)
            {
                string teksPertanyaan = cmbQuestionsList[i].Text.Trim();
                string teksJawaban = txtAnswersList[i].Text.Trim();

                if (!string.IsNullOrWhiteSpace(teksPertanyaan) && !string.IsNullOrWhiteSpace(teksJawaban))
                {
                    int finalQuestionId = -1;
                    bool isCustomQuestion = true;

                    if (cmbQuestionsList[i].SelectedValue != null && teksPertanyaan == cmbQuestionsList[i].GetItemText(cmbQuestionsList[i].SelectedItem))
                    {
                        finalQuestionId = (int)cmbQuestionsList[i].SelectedValue;
                        isCustomQuestion = false;
                    }
                    else
                    {
                        finalQuestionId = auth.GetOrCreateQuestionId(teksPertanyaan);
                    }

                    if (finalQuestionId != -1)
                    {
                        registerDto.RecoveryList.Add(new RecoveryItem
                        {
                            IdQuestion = finalQuestionId,
                            Answer = teksJawaban,
                            IsCustom = isCustomQuestion
                        });
                    }
                }
            }

            if (registerDto.RecoveryList.Count == 0)
            {
                MessageBox.Show("Anda harus mengisi minimal satu pertanyaan keamanan beserta jawabannya!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            auth.ChangeState(new SetupState());

            // Panggil method pemroses
            bool sukses = auth.UpdateState(registerDto);

            if (sukses)
            {
                this.Close();
            }
        }
    }
}