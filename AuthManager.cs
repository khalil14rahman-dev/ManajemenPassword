using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public enum AppState { SETUP, LOGIN, DASHBOARD }

public class UserCredentials
{
    public string PasswordHash { get; set; }
    public string SecurityQuestion { get; set; }
    public string AnswerHash { get; set; }
}

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

    private string path = "master_key9.txt"; 

    private readonly object _fileLock = new object(); // Objek pengunci untuk sinkronisasi thread

    private int loginAttempts = 0; // Pencatat jumlah salah

    private const string logFilePath = "activity_logs.json";

    private readonly DataRepository<LogActivity> _logRepo = DataRepository<LogActivity>.GetInstance(logFilePath);

    private IAuthState _currentState;

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
        _currentState = File.Exists(path) ? (IAuthState)new LoginState() : new SetupState();
    }

    public void ChangeState(IAuthState newState)
    {
        _currentState = newState;
    }

    public string GetPath() => path;
    public object GetFileLock() => _fileLock;
    public void IncrementAttempts() => loginAttempts++;

    public void SaveLog(string activity, string status)
    {
        List<LogActivity> logs = _logRepo.LoadData();

        string waktuSekarang = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        logs.Add(new LogActivity(waktuSekarang, activity, status));

        _logRepo.SaveData(logs);
    }

    public List<LogActivity> GetLogs()
    {
        return _logRepo.LoadData();
    }

    //Implementation of DTO 
    public bool UpdateState(PasswordRequestDto dto)
    {

        if (dto == null || !dto.IsValid())
        {
            return false;
        }

        try
        {
            return _currentState.Handle(this, dto);
        }
        catch (IOException)
        {
            throw;
        }
    }

    // Fungsi untuk mengecek apakah user sudah salah 3 kali
    public bool IsLockedOut()
    {
        return loginAttempts >= 3;
    }

    // Fungsi untuk mereset hitungan setelah jeda waktu selesai
    public void ResetAttempts()
    {
        loginAttempts = 0;
    }


    public void ChangePassword(string newPassword)
    {
        // SECURE CODING (Guard Clause / Early Exit): 
        // Mencegah parameter kosong masuk ke tingkat penyimpanan data file fisik
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
        {
            throw new ArgumentException("Keamanan Ditolak: Password baru tidak memenuhi syarat minimum.");
        }

        lock (_fileLock)
        {
            try
            {
                UserCredentials creds = new UserCredentials();
                if (File.Exists(path))
                {
                    string jsonLama = File.ReadAllText(path);
                    creds = JsonSerializer.Deserialize<UserCredentials>(jsonLama) ?? new UserCredentials();
                }

                // Update hash password saja, pertanyaan & jawaban keamanan tetap dipertahankan
                creds.PasswordHash = SecurityHelper.HashPassword(newPassword);

                string jsonBaru = JsonSerializer.Serialize(creds);
                File.WriteAllText(path, jsonBaru);
                SaveLog("Change Master Key", "Success");
            }
            catch (IOException)
            {
                SaveLog("Change Master Key (File Blocked)", "Failed");
                throw;
            }
        }
    }

    // FITUR LOGOUT: Mengembalikan status dari DASHBOARD ke LOGIN
    public void Logout()
    {
        if (CurrentState == AppState.DASHBOARD)
        {
            ChangeState(new LoginState());
            SaveLog("User Logout", "Success");
        }
    }

    // 1. Ambil data Pertanyaan Keamanan dari file untuk ditampilkan di UI Lupa Password
    public string GetSecurityQuestion()
    {
        lock (_fileLock)
        {
            if (!File.Exists(path)) return null;
            try
            {
                string json = File.ReadAllText(path);
                var creds = JsonSerializer.Deserialize<UserCredentials>(json);
                return creds?.SecurityQuestion;
            }
            catch { return null; }
        }
    }

    // 2. Validasi jawaban pengguna saat menempuh alur Lupa Password
    public bool ValidateRecoveryAnswer(string inputAnswer)
    {
        if (string.IsNullOrWhiteSpace(inputAnswer)) return false;

        lock (_fileLock)
        {
            if (!File.Exists(path)) return false;

            try
            {
                string json = File.ReadAllText(path);
                var creds = JsonSerializer.Deserialize<UserCredentials>(json);

                // Lakukan standarisasi teks input (lowercase & tanpa spasi luar) sebelum di-hash
                string hashedInputAnswer = SecurityHelper.HashPassword(inputAnswer.Trim().ToLower());

                if (creds != null && creds.AnswerHash == hashedInputAnswer)
                {
                    ChangeState(new SetupState()); // Izinkan balik ke SETUP untuk buat password baru
                    SaveLog("Forgot Password Verification", "Success");
                    return true;
                }

                SaveLog("Forgot Password Verification", "Failed");
                return false;
            }
            catch { return false; }
        }
    }
}