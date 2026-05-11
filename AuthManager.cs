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
 
    public bool UpdateState(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false; 
        }
        else if (CurrentState == AppState.SETUP && input.Length < 8)
        {
            return false; 
        }

        string hashedInput = SecurityHelper.HashPassword(input);

        switch (CurrentState)
        {
            case AppState.SETUP:
                File.WriteAllText(path, hashedInput);
                SaveLog("Setup Master Key", "Success"); //savelog
                CurrentState = AppState.LOGIN; // transisi ke login 
                return true;

            case AppState.LOGIN:
                if (File.Exists(path) && hashedInput == File.ReadAllText(path))
                {
                    SaveLog("User Login", "Success"); //savelog
                    loginAttempts = 0; // Reset jika sukses
                    CurrentState = AppState.DASHBOARD; //transisi ke dashboard
                    return true;
                }
                SaveLog("Login Attempt", "Failed"); //savelog
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