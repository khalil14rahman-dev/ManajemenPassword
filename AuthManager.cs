using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
//automata
public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{

    private string path = "master_key9.txt"; //bbb 

    private readonly object _fileLock = new object(); // Objek pengunci untuk sinkronisasi thread

    private int loginAttempts = 0; // Pencatat jumlah salah
    private const string logFilePath = "activity_logs.json"; 

    private DataRepository<LogActivity> logRepo = new DataRepository<LogActivity>("activity_logs.json");

    public AppState CurrentState { get; private set; }

    public AuthManager()
    {
        CurrentState = File.Exists(path) ? AppState.LOGIN : AppState.SETUP;
    }

    public void SaveLog(string activity, string status)
    {
        List<LogActivity> logs = logRepo.LoadData();

        logs.Add(new LogActivity(activity, status));

        logRepo.SaveData(logs);
    }

    public List<LogActivity> GetLogs()
    {
        return logRepo.LoadData();
    }
    
    //DTO
    public bool UpdateState(PasswordRequestDto dto)
    {
        // pastikan paket data tidak kosong dan isinya valid
        if (dto == null || !dto.IsValid())
        {
            return false;
        }

        // Ambil string di dalam DTO untuk di-hash
        string hashedInput = SecurityHelper.HashPassword(dto.Password);

        // SINKRONISASI: Semua proses baca/tulis di bawah wajib mengantre dengan rapi
        lock (_fileLock)
        {
            try
            {
                switch (CurrentState)
                {
                    case AppState.SETUP:
                        File.WriteAllText(path, hashedInput); // Proses Tulis Aman
                        SaveLog("Setup Master Key", "Success");
                        CurrentState = AppState.LOGIN;
                        return true;

                    case AppState.LOGIN:
                        // Proses Baca Aman
                        if (File.Exists(path) && hashedInput == File.ReadAllText(path))
                        {
                            SaveLog("User Login", "Success");
                            loginAttempts = 0;
                            CurrentState = AppState.DASHBOARD;
                            return true;
                        }
                        SaveLog("Login Attempt", "Failed");
                        loginAttempts++;
                        return false;

                    default:
                        return false;
                }
            }
            catch (IOException ex)
            {
                // DEFENSIVE: Jika file bentrok saat dibaca/ditulis, aplikasi tidak akan crash
                System.Windows.Forms.MessageBox.Show("Sistem mendeteksi bentrok pada file penyimpanan. Silakan coba lagi. Detail: " + ex.Message, "Sistem Sibuk");
                return false;
            }
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
        string hashedNew = SecurityHelper.HashPassword(newPassword);

        // SINKRONISASI: Memaksa proses lain mengantre saat proses menulis sedang berjalan
        lock (_fileLock)
        {
            try
            {
                File.WriteAllText(path, hashedNew);
                SaveLog("Change Master Key", "Success");
            }
            catch (IOException ex)
            {
                // DEFENSIVE: Menangkap error jika file mendadak dikunci oleh sistem operasi
                SaveLog("Change Master Key (File Blocked)", "Failed");
                System.Windows.Forms.MessageBox.Show("Gagal menulis data. File sedang digunakan oleh proses lain: " + ex.Message, "Error File");
            }
        }
    }

    // FITUR LOGOUT: Mengembalikan status dari DASHBOARD ke LOGIN
    public void Logout()
    {
        if (CurrentState == AppState.DASHBOARD)
        {
            CurrentState = AppState.LOGIN;
            SaveLog("User Logout", "Success");
        }
    }

    // FITUR LUPA PASSWORD: Menghapus file master key lama agar user bisa setup ulang dari awal
    public bool ResetMasterKey()
    {
        // Menggunakan lock agar proses penghapusan file tidak bentrok
        lock (_fileLock)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path); // Hapus file password lama
                }
                CurrentState = AppState.SETUP; // Kembalikan state ke SETUP awal
                SaveLog("Reset Master Key (Lupa Password)", "Success");
                loginAttempts = 0; // Reset hitungan salah login
                return true;
            }
            catch (IOException ex)
            {
                System.Windows.Forms.MessageBox.Show("Gagal mereset password, file sedang digunakan: " + ex.Message, "Error");
                return false;
            }
        }
    }
}