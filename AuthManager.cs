using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.IO;

public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{
    private string path = "master_key10.txt";
    private const string logFilePath = "activity_logs.json";

    private readonly DataRepository<LogActivity> _logRepo = DataRepository<LogActivity>.GetInstance(logFilePath);

    public AppState CurrentState { get; private set; }

    public AuthManager()
    {
        CurrentState = File.Exists(path) ? AppState.LOGIN : AppState.SETUP;
    }

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

    // UpdateState 
    public bool UpdateState(string input)
    {
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