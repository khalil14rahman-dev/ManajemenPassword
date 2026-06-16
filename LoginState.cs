using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Project_KPL_ManajemenPassword
{
    public class LoginState : IAuthState
    {
        public bool Handle(AuthManager context, PasswordRequestDto dto)
        {
            string hashedInput = SecurityHelper.HashPassword(dto.Password);

            lock (context.GetFileLock())
            {
                string storedPath = context.GetPath();
                if (File.Exists(storedPath))
                {
                    try
                    {
                        string json = File.ReadAllText(storedPath);
                        var creds = System.Text.Json.JsonSerializer.Deserialize<UserCredentials>(json);

                        // Cocokkan hash input dengan PasswordHash di dalam objek JSON
                        if (creds != null && hashedInput == creds.PasswordHash)
                        {
                            context.SaveLog("User Login", "Success");
                            context.ResetAttempts();
                            context.ChangeState(new DashboardState());
                            return true;
                        }
                    }
                    catch
                    {
                        // Menangani jika file corrupt atau format lama
                    }
                }

                context.SaveLog("Login Attempt", "Failed");
                context.IncrementAttempts();
                return false;
            }
        }
    }
}
