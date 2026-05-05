using Project_KPL_ManajemenPassword;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{
    private string path = "master_key6.txt";
    private const string logFilePath = "activity_logs.json";
    public AppState CurrentState { get; private set; }

    public AuthManager()
    {
        CurrentState = File.Exists(path) ? AppState.LOGIN : AppState.SETUP;
    }

    // FUNGSI PENCATAT LOG (JSON)
    public void SaveLog(string activity, string status)
    {
        List<LogActivity> logs = new List<LogActivity>();

        if (File.Exists(logFilePath))
        {
            string existingJson = File.ReadAllText(logFilePath);
            logs = JsonSerializer.Deserialize<List<LogActivity>>(existingJson) ?? new List<LogActivity>();
        }

        logs.Add(new LogActivity(activity, status));

        string updatedJson = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logFilePath, updatedJson);
    } // Kurung penutup SaveLog ada di sini

    // Fungsi untuk mengambil semua riwayat log (Wajib PUBLIC agar terbaca di Form lain)
    public List<LogActivity> GetLogs()
    {
        if (!File.Exists(logFilePath)) return new List<LogActivity>();

        try
        {
            string json = File.ReadAllText(logFilePath);
            return JsonSerializer.Deserialize<List<LogActivity>>(json) ?? new List<LogActivity>();
        }
        catch
        {
            return new List<LogActivity>();
        }
    }

    // LOGIKA PERUBAHAN HALAMAN (AUTOMATA)
    public bool UpdateState(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        switch (CurrentState)
        {
            case AppState.SETUP:
                File.WriteAllText(path, input);
                SaveLog("Setup Master Key", "Success");
                CurrentState = AppState.LOGIN;
                return true;

            case AppState.LOGIN:
                if (File.Exists(path) && input == File.ReadAllText(path))
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
        File.WriteAllText(path, newPassword);
        SaveLog("Change Master Key", "Success");
    }
}