using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project_KPL_ManajemenPassword
{
    public class SetupState : IAuthState
    {
        public bool Handle(AuthManager context, PasswordRequestDto dto)
        {
            // Pastikan DTO membawa data recovery questions saat setup awal
            if (string.IsNullOrWhiteSpace(dto.SecurityQuestion) || string.IsNullOrWhiteSpace(dto.SecurityAnswer))
            {
                return false;
            }

            string hashedPass = SecurityHelper.HashPassword(dto.Password);
            string hashedAnswer = SecurityHelper.HashPassword(dto.SecurityAnswer.Trim().ToLower());

            lock (context.GetFileLock())
            {
                UserCredentials newCreds = new UserCredentials
                {
                    PasswordHash = hashedPass,
                    SecurityQuestion = dto.SecurityQuestion,
                    AnswerHash = hashedAnswer
                };

                string json = System.Text.Json.JsonSerializer.Serialize(newCreds);
                File.WriteAllText(context.GetPath(), json);

                context.SaveLog("Setup Master Key & Questions", "Success");
                context.ChangeState(new LoginState());
                return true;
            }
        }
    }
}
