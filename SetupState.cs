using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project_KPL_ManajemenPassword
{
    public class SetupState : IAuthState
    {
        public bool Handle(AuthManager context, PasswordRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                MessageBox.Show("Username dan Password wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dto.RecoveryList == null || dto.RecoveryList.Count == 0)
            {
                MessageBox.Show("Anda harus menambahkan minimal satu pertanyaan keamanan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DatabaseConnection db = DatabaseConnection.GetInstance();
            try
            {
                db.OpenConnection();

                string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, db.GetConnection()))
                {
                    checkCmd.Parameters.AddWithValue("@username", dto.Username);
                    int userExist = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (userExist > 0)
                    {
                        MessageBox.Show("Username tersebut sudah terdaftar! Gunakan nama lain.", "Registrasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }

                string hashedMasterPassword = SecurityHelper.HashPassword(dto.Password);

                string queryUser = @"INSERT INTO users (username, master_password, is_active) 
                                     VALUES (@username, @master_password, 1)";

                using (MySqlCommand cmd = new MySqlCommand(queryUser, db.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@username", dto.Username);
                    cmd.Parameters.AddWithValue("@master_password", hashedMasterPassword);
                    cmd.ExecuteNonQuery();
                }

                string getIdQuery = "SELECT LAST_INSERT_ID()";
                MySqlCommand getIdCmd = new MySqlCommand(getIdQuery, db.GetConnection());
                int newIdUser = Convert.ToInt32(getIdCmd.ExecuteScalar());

                string queryPivot = @"INSERT INTO user_security_questions (id_user, id_question, security_answer) 
                                      VALUES (@id_user, @id_question, @security_answer)";

                foreach (var item in dto.RecoveryList)
                {
                    using (MySqlCommand cmdPivot = new MySqlCommand(queryPivot, db.GetConnection()))
                    {
                        cmdPivot.Parameters.AddWithValue("@id_user", newIdUser);
                        cmdPivot.Parameters.AddWithValue("@id_question", item.IdQuestion);
                        cmdPivot.Parameters.AddWithValue("@security_answer", item.Answer.Trim());
                        cmdPivot.ExecuteNonQuery();
                    }
                }

                context.SetSession(newIdUser, dto.Username);
                context.SaveLog("Registrasi Akun Baru Berhasil", "Success");

                MessageBox.Show("Akun berhasil dibuat! Silakan masuk ke aplikasi.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                context.ChangeState(new LoginState());
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan pendaftaran ke MySQL: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}