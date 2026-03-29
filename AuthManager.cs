using System;
using System.IO; // Penting untuk urusan simpan file

public class AuthManager
{
    // Nama file rahasia kita
    private string path = "master_key.txt";

    // Cek: Ini pertama kali pakai atau sudah ada password?
    public bool IsFirstTime()
    {
        return !File.Exists(path);
    }

    // Fungsi simpan password baru
    public void SimpanPasswordBaru(string password)
    {
        File.WriteAllText(path, password);
    }

    // Fungsi cek password saat login
    public bool CekPassword(string input)
    {
        if (File.Exists(path))
        {
            string passAsli = File.ReadAllText(path);
            return input == passAsli;
        }
        return false;
    }
}