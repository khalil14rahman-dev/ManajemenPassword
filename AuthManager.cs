using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{
    private static AuthManager _instance;
    private static readonly object _instanceLock = new object();

    public static AuthManager GetInstance()
    {
        lock (_instanceLock)
        {
            if (_instance == null)
            {
                _instance = new AuthManager();
            }
            return _instance;
        }
    }

    private int loginAttempts = 0;
    private IAuthState _currentState;

    public int CurrentIdUser { get; private set; } = -1;
    public string CurrentUsername { get; private set; } = "";

    public AppState CurrentState
    {
        get
        {
            if (_currentState is SetupState) return AppState.SETUP;
            if (_currentState is LoginState) return AppState.LOGIN;
            return AppState.DASHBOARD;
        }
    }

    private AuthManager()
    {
        _currentState = new LoginState();
    }

    public void ChangeState(IAuthState newState)
    {
        _currentState = newState;
    }

    public void IncrementAttempts() => loginAttempts++;
    public void SetSession(int idUser, string username)
    {
        this.CurrentIdUser = idUser;
        this.CurrentUsername = username;
    }

    public void ClearSession()
    {
        this.CurrentIdUser = -1;
        this.CurrentUsername = "";
    }
    public void SaveLog(string activity, string status)
    {
        int logUserId = (CurrentIdUser != -1) ? CurrentIdUser : 0;

        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();
            string query = "INSERT INTO logs (id_user, aktivitas, status) VALUES (@id_user, @aktivitas, @status)";

            using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@id_user", logUserId);
                cmd.Parameters.AddWithValue("@aktivitas", activity);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("CRITICAL LOG ERROR: " + ex.Message);
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public bool ValidateLogin(string inputUsername, string inputPassword)
    {
        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();

            string query = @"SELECT u.id_user, u.master_password, COUNT(usq.id_question) as total_pertanyaan
                             FROM users u
                             LEFT JOIN user_security_questions usq ON u.id_user = usq.id_user
                             WHERE u.username = @username AND u.is_active = 1
                             GROUP BY u.id_user";

            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            cmd.Parameters.AddWithValue("@username", inputUsername);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string storedHash = reader["master_password"].ToString();
                    int totalPertanyaan = Convert.ToInt32(reader["total_pertanyaan"]);

                    if (SecurityHelper.HashPassword(inputPassword) == storedHash)
                    {
                        if (totalPertanyaan == 0)
                        {
                            MessageBox.Show("Login ditolak: Akun Anda belum mengatur pertanyaan keamanan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        int idUser = Convert.ToInt32(reader["id_user"]);
                        SetSession(idUser, inputUsername);
                        return true;
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public bool IsLockedOut() => loginAttempts >= 3;

    public void ResetAttempts() => loginAttempts = 0;

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
        {
            throw new ArgumentException("Keamanan Ditolak: Password baru tidak memenuhi syarat minimum.");
        }

        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();
            string hashedPassword = SecurityHelper.HashPassword(newPassword);

            string query = "UPDATE users SET master_password = @password WHERE id_user = @id_user";

            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@id_user", CurrentIdUser);

            cmd.ExecuteNonQuery();
            SaveLog("Change Master Key", "Success");
        }
        catch (Exception)
        {
            SaveLog("Change Master Key", "Failed");
            throw;
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public void Logout()
    {
        if (CurrentState == AppState.DASHBOARD)
        {
            SaveLog("User Logout", "Success");
            ClearSession();
            ChangeState(new LoginState());
        }
    }
    public List<KeyValuePair<int, string>> GetAllSecurityQuestions(string inputUsername)
    {
        List<KeyValuePair<int, string>> questions = new List<KeyValuePair<int, string>>();
        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();

            string query = @"SELECT sq.id_question, sq.text_question 
                             FROM users u 
                             INNER JOIN user_security_questions usq ON u.id_user = usq.id_user
                             INNER JOIN security_questions sq ON usq.id_question = sq.id_question 
                             WHERE u.username = @username AND u.is_active = 1";

            using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@username", inputUsername);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questions.Add(new KeyValuePair<int, string>(
                            Convert.ToInt32(reader["id_question"]),
                            reader["text_question"].ToString()
                        ));
                    }
                }
            }
            return questions;
        }
        catch { return questions; }
        finally { db.CloseConnection(); }
    }
    public bool ValidateAllRecoveryAnswers(int userId, List<KeyValuePair<int, string>> userAnswers)
    {
        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();

            foreach (var item in userAnswers)
            {
                string query = @"SELECT security_answer FROM user_security_questions 
                                 WHERE id_user = @id_user AND id_question = @id_question";

                using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@id_user", userId);
                    cmd.Parameters.AddWithValue("@id_question", item.Key);

                    object result = cmd.ExecuteScalar();
                    if (result == null) return false;

                    if (!string.Equals(result.ToString().Trim(), item.Value.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }

            ChangeState(new SetupState());
            SaveLog("Forgot Password Verification", "Success");
            return true;
        }
        catch { return false; }
        finally { db.CloseConnection(); }
    }

    public int GetUserIdByUsername(string username)
    {
        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();
            string query = "SELECT id_user FROM users WHERE username = @username AND is_active = 1";
            using (MySqlCommand cmd = new MySqlCommand(query, db.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@username", username);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }
        catch { return -1; }
        finally { db.CloseConnection(); }
    }

    public bool UpdateState(PasswordRequestDto dto)
    {
        if (_currentState == null) return false;
        return _currentState.Handle(this, dto);
    }

    public List<LogActivity> GetLogs()
    {
        List<LogActivity> listLogs = new List<LogActivity>();
        DatabaseConnection db = DatabaseConnection.GetInstance();

        try
        {
            db.OpenConnection();
            string query = "SELECT * FROM logs WHERE id_user = @id_user ORDER BY id_log DESC";
            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            cmd.Parameters.AddWithValue("@id_user", CurrentIdUser);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string waktu = Convert.ToDateTime(reader["created_at"]).ToString("yyyy-MM-dd HH:mm:ss");
                    string aktivitas = reader["aktivitas"].ToString();
                    string status = reader["status"].ToString();

                    listLogs.Add(new LogActivity(waktu, aktivitas, status));
                }
            }
            return listLogs;
        }
        catch
        {
            return new List<LogActivity>();
        }
        finally
        {
            db.CloseConnection();
        }
    }

    public int GetOrCreateQuestionId(string questionText)
    {
        if (string.IsNullOrWhiteSpace(questionText)) return -1;

        DatabaseConnection db = DatabaseConnection.GetInstance();
        try
        {
            db.OpenConnection();
            string checkQuery = "SELECT id_question FROM security_questions WHERE LOWER(TRIM(text_question)) = LOWER(TRIM(@text))";
            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, db.GetConnection()))
            {
                checkCmd.Parameters.AddWithValue("@text", questionText);
                object result = checkCmd.ExecuteScalar();

                if (result != null) return Convert.ToInt32(result);
            }
            string insertQuery = "INSERT INTO security_questions (text_question) VALUES (@text); SELECT LAST_INSERT_ID();";
            using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, db.GetConnection()))
            {
                insertCmd.Parameters.AddWithValue("@text", questionText);
                return Convert.ToInt32(insertCmd.ExecuteScalar());
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error GetOrCreateQuestionId: " + ex.Message);
            return -1;
        }
        finally
        {
            db.CloseConnection();
        }
    }
}