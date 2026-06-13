using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{
    private string path = "master_key11.txt"; 
    private const string logFilePath = "activity_logs.json";

    
    private int loginAttempts = 0;

    DataRepository<LogActivity> logRepo = DataRepository<LogActivity>.GetInstance("data_log.json"); public AppState CurrentState { get; private set; }

    public AuthManager()
    {
        CurrentState = File.Exists(path) ? AppState.LOGIN : AppState.SETUP;
    }

    // LOG (JSON)
    public void SaveLog(string activity, string status)
    {
        // 1. Ambil data 
        List<LogActivity> logs = logRepo.LoadData();

        // 2. Tambah data baru
        string waktuSekarang = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        logs.Add(new LogActivity(waktuSekarang, activity, status));

        logRepo.SaveData(logs);
    }

    public List<LogActivity> GetLogs()
    {
        return logRepo.LoadData();
    }

    // UpdateState 
    public bool UpdateState(string input)
    {
        // STRATEGI DEFENSIVE PROGRAMMING (Logic Level)
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }
        else if (CurrentState == AppState.SETUP && input.Length < 8)
        {
            return false;
        }

        // Defensive Programming
        if (string.IsNullOrWhiteSpace(input)) return false;

        string hashedInput = SecurityHelper.HashPassword(input);

        switch (CurrentState)
        {
            case AppState.SETUP:
                File.WriteAllText(path, hashedInput);
                SaveLog("Setup Master Key", "Success");
                CurrentState = AppState.LOGIN;
                return true;

            case AppState.LOGIN:
                if (File.Exists(path) && hashedInput == File.ReadAllText(path))
                {
                    SaveLog("User Login", "Success");
                    loginAttempts = 0; // Reset jika sukses
                    CurrentState = AppState.DASHBOARD;
                    return true;
                }
                SaveLog("Login Attempt", "Failed");
                loginAttempts++; // Tambah hitungan jika salah
                return false;

            default:
                return false;
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
        File.WriteAllText(path, hashedNew);
        SaveLog("Change Master Key", "Success");
    }
}