using System;
using System.IO;

// Definisi State untuk Automata
public enum AppState { SETUP, LOGIN, DASHBOARD }

public class AuthManager
{
    //master 5 ; aku
    private string path = "master_key6.txt";
    public AppState CurrentState { get; private set; }

    public AuthManager()
    {
        // State awal ditentukan dari keberadaan file (Inisialisasi State)
        CurrentState = File.Exists(path) ? AppState.LOGIN : AppState.SETUP;
    }

    // Fungsi utama transisi state (Logika Automata)
    public bool UpdateState(string input)
    {
        // Defensive Programming: Validasi tingkat Logic
        if (string.IsNullOrWhiteSpace(input)) return false;

        switch (CurrentState)
        {
            case AppState.SETUP:
                File.WriteAllText(path, input);
                CurrentState = AppState.LOGIN; // Transisi ke LOGIN
                return true;

            case AppState.LOGIN:
                if (File.Exists(path) && input == File.ReadAllText(path))
                {
                    CurrentState = AppState.DASHBOARD; // Transisi ke DASHBOARD
                    return true;
                }
                return false; // Password salah

            default:
                return false;
        }
    }
}