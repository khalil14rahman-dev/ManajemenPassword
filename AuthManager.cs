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
    private const string logFilePath = "activity_logs.json"; //savelog JSON path
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
            try
            {
                string existingJson = File.ReadAllText(logFilePath);
                logs = JsonSerializer.Deserialize<List<LogActivity>>(existingJson) ?? new List<LogActivity>();
            }
            catch { logs = new List<LogActivity>(); }
        }

        logs.Add(new LogActivity(activity, status));
        string updatedJson = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logFilePath, updatedJson);
    }

    public List<LogActivity> GetLogs()
    {
        if (!File.Exists(logFilePath)) return new List<LogActivity>();
        try
        {
            string json = File.ReadAllText(logFilePath);
            return JsonSerializer.Deserialize<List<LogActivity>>(json) ?? new List<LogActivity>();
        }
        catch { return new List<LogActivity>(); }
    }

    // SATU FUNGSI UpdateState (Sudah Gabungan Hashing + Log + Automata)
    public bool UpdateState(string input)
    {
        // STRATEGI DEFENSIVE PROGRAMMING (Logic Level)
        if (string.IsNullOrWhiteSpace(input))
        {
            return false; // Kondisi 1: Tolak input kosong
        }
        else if (CurrentState == AppState.SETUP && input.Length < 8)
        {
            return false; // Kondisi 2: Tolak eksekusi jika tidak memenuhi standar keamanan saat SETUP
        }

        // Hash input panggil dari kelas security helper
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
                    CurrentState = AppState.DASHBOARD; //transisi ke dashboard
                    return true;
                }
                SaveLog("Login Attempt", "Failed"); //savelog
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