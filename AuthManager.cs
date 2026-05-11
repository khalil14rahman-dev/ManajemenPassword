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
        // STRATEGI DEFENSIVE PROGRAMMING (Logic Level)
        if (string.IsNullOrWhiteSpace(input))
        {
            return false; // Kondisi 1: Tolak input kosong
        }
        else if (CurrentState == AppState.SETUP && input.Length < 8)
        {
            return false; // Kondisi 2: Tolak eksekusi jika tidak memenuhi standar keamanan saat SETUP
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