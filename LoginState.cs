using System;
using System.Windows.Forms;

namespace Project_KPL_ManajemenPassword
{
    public class LoginState : IAuthState
    {
        public bool Handle(AuthManager context, PasswordRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool isPasswordValid = context.ValidateLogin(dto.Username, dto.Password);

            if (isPasswordValid)
            {
                context.ResetAttempts();
                context.SaveLog("User Login Berhasil", "Success");

                context.ChangeState(new DashboardState());
                return true;
            }
            else
            {
                context.IncrementAttempts();

                context.SaveLog($"Gagal Login (Percobaan Username: {dto.Username})", "Failed");
                return false;
            }
        }
    }
}