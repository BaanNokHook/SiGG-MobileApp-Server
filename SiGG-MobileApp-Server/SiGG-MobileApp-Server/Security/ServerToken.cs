using SiGG_MobileApp_Server.ManagedServices;
using SocketAppServer.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Security
{
    ///added comment
    public class ServerToken
    {
        public Guid SessionId { get; private set; }

        internal string RemoteIP { get; private set; }

        public ServerUser User { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime ExpireAt { get; private set; }

        public string UserToken { get; private set; }

        public bool IsReplicated { get; private set; }

        public bool HasExpired()
        {
            return (ExpireAt < DateTime.Now);
        }

        private List<UserActivity> userActivities;

        public IReadOnlyCollection<UserActivity> GetActivities()
        {
            return userActivities.AsReadOnly();
        }

        public void RegisterActivity(UserActivity activity)
        {
            userActivities.Add(activity);
        }

        private string cryptoPasswd;

        ISecurityManagementService securityManagementService = null;
        public ServerToken(ServerUser user,
            ref SocketRequest request)
        {
            Fill(user);
            CreateToken();
            RemoteIP = request.RemoteEndPoint.Address.ToString();
        }

        public ServerToken(string token, ServerUser user, string remoteEndPoint)
        {
            UserToken = token;
            User = user;
            RemoteIP = remoteEndPoint;
        }

        private void Fill(ServerUser user)
        {
            IServiceManager manager = ServiceManager.GetInstance();
            securityManagementService = manager.GetService<ISecurityManagementService>();

            userActivities = new List<UserActivity>();
            User = user;
            SessionId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            ExpireAt = CreatedAt.AddMinutes(securityManagementService.GetDefinitions().TokenLifeTime);
        }

        public ServerToken(string token)
        {
            IsReplicated = true;
            Fill(new ServerUser("replicated", "replicated", "replicated", "replicated"));
            UserToken = token;
        }

        private string CreateContentString()
        {
            string str = $"{SessionId.ToString()}";
            if (!string.IsNullOrEmpty(User.Identifier))
                str += $"|{User.Identifier}";
            if (!string.IsNullOrEmpty(User.Name))
                str += $"|{User.Name}";
            if (!string.IsNullOrEmpty(User.Email))
                str += $"|{User.Email}";
            if (!string.IsNullOrEmpty(User.Organization))
                str += $"|{User.Organization}";
            str += $"|{CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")}";
            str += $"|{ExpireAt.ToString("yyyy-MM-dd HH:mm:ss")}";
            str += $"|{RemoteIP}";
            return str;
        }

        private void CreateToken()
        {
            string str = CreateContentString();
            UserToken = new Crypto(str, GetCryptoPassword()).Crypt();
        }

        private string random = null;

        private string GetCryptoPassword()
        {
            string passwd = securityManagementService.GetDefinitions().TokenCryptPassword;
            if (string.IsNullOrEmpty(passwd))
            {
                if (!string.IsNullOrEmpty(cryptoPasswd))
                    return cryptoPasswd;

                string guid = SessionId.ToString().Replace("-", "");

                string str = CreateContentString();
                byte[] sha256 = Encoding.ASCII.GetBytes(str);

                int lenght = sha256.Length;

                if (random == null)
                    random = new Random(lenght).Next().ToString();

                cryptoPasswd = $"{guid}{random}";
                return cryptoPasswd;
            }
            else return passwd;
        }
    }
}
