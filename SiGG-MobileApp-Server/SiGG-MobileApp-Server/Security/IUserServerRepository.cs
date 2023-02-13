using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Security
{
    public interface IServerUserRepository
    {
        ServerUser Authenticate(string userNameOrEmail, string password);

        void OnSuccessFulAuthentication(string token);
    }
}
