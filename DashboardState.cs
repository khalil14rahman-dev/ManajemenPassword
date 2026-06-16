using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public class DashboardState : IAuthState
    {
        public bool Handle(AuthManager context, PasswordRequestDto dto)
        {
            return true;
        }
    }
}
