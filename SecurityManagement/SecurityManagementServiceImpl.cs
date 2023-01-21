// Satellite-Communication-Server //

using SocketAppServer.ManagedServices;
using SocketAppServer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketAppServer.CoreServices.SecurityManagement
{
    internal class SecurityManagementServiceImpl : ISecurityManagementService
    {
        private IControllerManager controllerManager;
        private IInterceptorManagerService interceptorManager;
        private ICoreServerService coreServer;
        public SecurityManagementServiceImpl()
        {
            IServiceManager manager = ServiceManager.GetInstance();
            controllerManager = manager.GetService<IControllerManager>();
            interceptorManager = manager.GetService<IInterceptorManagerService>();
            coreServer = ServiceManager.GetInstance().GetService<ICoreServerService>("realserver");
        }

        private bool securityEnabled = false;
        private BasicSecurityDefinitions definitions;
        public void EnableSecurity(IServerUserRepository userRepository, int tokenLifetime = 3, string tokenCryptPassword = "")
        {
            if (coreServer.IsLoadBalanceEnabled())
                throw new InvalidOperationException("Authentication features cannot be enabled on a load balancing server");

            if (securityEnabled)
                throw new Exception("Aready enabled.");

            controllerManager.RegisterController(typeof(AuthorizationController));
            interceptorManager.AddInterceptor(new SecurityTokenInterceptor());
            interceptorManager.AddInterceptor(new UserRoleValidationInterceptor());

            definitions = new BasicSecurityDefinitions(userRepository,
                tokenLifetime,
                tokenCryptPassword);

            securityEnabled = true;
        }

        public BasicSecurityDefinitions GetDefinitions()
        {
            return new BasicSecurityDefinitions(definitions.Repository,
                definitions.TokenLifeTime, "");
        }

        public IReadOnlyCollection<LoggedUserInfo> GetLoggedUsers()
        {
            List<LoggedUserInfo> result = new List<LoggedUserInfo>();
            TokenRepository.Instance.Tokens.ForEach(token =>
            result.Add(new LoggedUserInfo(
                token.SessionId,
                token.User.Identifier,
                token.User.Name,
                token.CreatedAt,
                token.ExpireAt))
            );
            return result.AsReadOnly();
        }

        public IReadOnlyCollection<UserActivity> GetUserActivities(LoggedUserInfo loggedUser)
        {
            ServerToken token = TokenRepository.Instance.GetToken(loggedUser.SessionId);
            if (token == null)
                return new List<UserActivity>().AsReadOnly();
            return token.GetActivities();
        }

        public ServerUser Authenticate(string userNameOrEmail, string password)
        {
            return definitions.Repository.Authenticate(userNameOrEmail, password);
        }

        public bool IsAuthenticationEnabled()
        {
            return securityEnabled;
        }

        public void OnSuccessFulAuthentication(string token)
        {
            definitions.Repository.OnSuccessFulAuthentication(token);
        }

        public LoggedUserInfo GetLoggedUser(string token)
        {
            var tk = TokenRepository.Instance.GetToken(token);
            if (tk == null)
                return null;

            return new LoggedUserInfo(tk.SessionId, tk.User.Identifier,
                  tk.User.Name, tk.CreatedAt, tk.ExpireAt);
        }
    }
}
