// DT Software //

using SocketAppServer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices
{
    public interface ISecurityManagementService
    {
        void EnableSecurity(IServerUserRepository userRepository,
                 int tokenLifetime = 3, string tokenCryptPassword = "");

        bool IsAuthenticationEnabled();

        BasicSecurityDefinitions GetDefinitions();

        IReadOnlyCollection<LoggedUserInfo> GetLoggedUsers();

        LoggedUserInfo GetLoggedUser(string token);

        IReadOnlyCollection<UserActivity> GetUserActivities(LoggedUserInfo loggedUser);

        ServerUser Authenticate(string userNameOrEmail, string password);

        void OnSuccessFulAuthentication(string token);
    }
}
