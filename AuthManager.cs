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
    private string path = "master_key9.txt"; 
    private const string logFilePath = "activity_logs.json";
    private DataRepository<LogActivity> logRepo = new DataRepository<LogActivity>("activity_logs.json");
    public AppState CurrentState { get; private set; }

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
        logs.Add(new LogActivity(activity, status));

        logRepo.SaveData(logs);
    }

    public List<LogActivity> GetLogs()
    {
        return logRepo.LoadData();
    }

    // UpdateState 
    public bool UpdateState(string input)
    {
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
                    CurrentState = AppState.DASHBOARD;
                    return true;
                }
                SaveLog("Login Attempt", "Failed");
                return false;

            default:
                return false;
        }
    }

    public void ChangePassword(string newPassword)
    {
        string hashedNew = SecurityHelper.HashPassword(newPassword);
        File.WriteAllText(path, hashedNew);
        SaveLog("Change Master Key", "Success");
    }
}