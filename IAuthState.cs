using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_KPL_ManajemenPassword
{
    public interface IAuthState
    {
        bool Handle(AuthManager context, PasswordRequestDto dto);
    }
}
